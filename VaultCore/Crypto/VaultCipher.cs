using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace VaultCore.Crypto;

/// <summary>
/// Key derivation settings stored in the vault file header.
/// </summary>
public sealed record VaultKdfParameters(string Algorithm, int Iterations, byte[] Salt)
{
    public bool SameAs(VaultKdfParameters? other) =>
        other is not null
        && Algorithm == other.Algorithm
        && Iterations == other.Iterations
        && CryptographicOperations.FixedTimeEquals(Salt, other.Salt);
}

/// <summary>
/// A parsed (still encrypted) v2 vault file.
/// </summary>
public sealed record VaultEnvelope(VaultKdfParameters Kdf, byte[] Nonce, byte[] Tag, byte[] Ciphertext);

/// <summary>
/// Vault file format v2.
/// <list type="bullet">
/// <item>Key derivation: PBKDF2-HMAC-SHA256, random 16-byte salt, iteration count stored in the header.</item>
/// <item>Encryption: AES-256-GCM, fresh random 12-byte nonce on every save.</item>
/// <item>The header (format, version, KDF parameters, cipher name) is authenticated as associated data,
/// so any change to the file is detected.</item>
/// </list>
/// </summary>
public static class VaultCipher
{
    public const string FormatName = "PasswordVault";
    public const int FormatVersion = 2;
    public const string KdfAlgorithm = "PBKDF2-HMAC-SHA256";
    public const string CipherAlgorithm = "AES-256-GCM";

    /// <summary>OWASP 2023 recommendation for PBKDF2-HMAC-SHA256.</summary>
    public const int DefaultIterations = 600_000;

    /// <summary>Upper bound accepted when reading a file, to avoid a hostile file freezing the app.</summary>
    public const int MaxIterations = 10_000_000;

    public const int KeySize = 32;
    private const int SaltSize = 16;
    private const int NonceSize = 12;
    private const int TagSize = 16;

    public static VaultKdfParameters NewKdfParameters(int iterations = DefaultIterations)
    {
        if (iterations < 1 || iterations > MaxIterations)
            throw new ArgumentOutOfRangeException(nameof(iterations));

        return new VaultKdfParameters(KdfAlgorithm, iterations, RandomNumberGenerator.GetBytes(SaltSize));
    }

    /// <summary>
    /// Derive the 256-bit encryption key from the master password.
    /// Deliberately slow: this is what makes offline brute force expensive.
    /// </summary>
    public static byte[] DeriveKey(string password, VaultKdfParameters kdf)
    {
        ArgumentNullException.ThrowIfNull(password);
        if (kdf.Algorithm != KdfAlgorithm)
            throw new VaultFormatException($"Unsupported key derivation '{kdf.Algorithm}'.");

        return Rfc2898DeriveBytes.Pbkdf2(password, kdf.Salt, kdf.Iterations, HashAlgorithmName.SHA256, KeySize);
    }

    /// <summary>
    /// Encrypt <paramref name="plaintext"/> and return the complete vault file content.
    /// </summary>
    public static byte[] Seal(ReadOnlySpan<byte> plaintext, byte[] key, VaultKdfParameters kdf)
    {
        byte[] nonce = RandomNumberGenerator.GetBytes(NonceSize);
        byte[] tag = new byte[TagSize];
        byte[] ciphertext = new byte[plaintext.Length];

        using (AesGcm aes = new(key, TagSize))
            aes.Encrypt(nonce, plaintext, ciphertext, tag, AssociatedData(kdf));

        VaultFileModel model = new()
        {
            Format = FormatName,
            Version = FormatVersion,
            Kdf = new VaultFileKdf
            {
                Algorithm = kdf.Algorithm,
                Iterations = kdf.Iterations,
                Salt = Convert.ToBase64String(kdf.Salt),
            },
            Cipher = new VaultFileCipher
            {
                Algorithm = CipherAlgorithm,
                Nonce = Convert.ToBase64String(nonce),
                Tag = Convert.ToBase64String(tag),
            },
            Payload = Convert.ToBase64String(ciphertext),
        };

        return JsonSerializer.SerializeToUtf8Bytes(model, VaultJsonContext.Default.VaultFileModel);
    }

    /// <summary>
    /// True if <paramref name="content"/> looks like a v2 vault file (does not decrypt it).
    /// </summary>
    public static bool IsVaultFile(ReadOnlySpan<byte> content)
    {
        try
        {
            Parse(content);
            return true;
        }
        catch (VaultFormatException)
        {
            return false;
        }
    }

    /// <summary>
    /// Parse and validate the vault file header.
    /// </summary>
    /// <exception cref="VaultFormatException">Not a valid v2 vault file.</exception>
    public static VaultEnvelope Parse(ReadOnlySpan<byte> content)
    {
        VaultFileModel? model;
        try
        {
            // Only the first JSON value counts: some storage providers and transfer tools rewrite a file
            // without truncating it, leaving stale bytes at the end. Everything that matters
            // (header and payload) is inside the object and authenticated by AES-GCM.
            Utf8JsonReader reader = new(content);
            if (reader.Read())
                reader.Skip();
            int length = (int)reader.BytesConsumed;
            model = JsonSerializer.Deserialize(content[..length], VaultJsonContext.Default.VaultFileModel);
        }
        catch (JsonException exception)
        {
            throw new VaultFormatException("The file is not a Password Vault v2 file.", exception);
        }

        if (model is null || model.Format != FormatName)
            throw new VaultFormatException("The file is not a Password Vault v2 file.");
        if (model.Version != FormatVersion)
            throw new VaultFormatException($"Unsupported vault version {model.Version}.");
        if (model.Kdf is null || model.Kdf.Algorithm != KdfAlgorithm)
            throw new VaultFormatException("Unsupported key derivation.");
        if (model.Kdf.Iterations < 1 || model.Kdf.Iterations > MaxIterations)
            throw new VaultFormatException("Invalid key derivation iteration count.");
        if (model.Cipher is null || model.Cipher.Algorithm != CipherAlgorithm)
            throw new VaultFormatException("Unsupported cipher.");

        try
        {
            byte[] salt = Convert.FromBase64String(model.Kdf.Salt ?? "");
            byte[] nonce = Convert.FromBase64String(model.Cipher.Nonce ?? "");
            byte[] tag = Convert.FromBase64String(model.Cipher.Tag ?? "");
            byte[] ciphertext = Convert.FromBase64String(model.Payload ?? "");

            if (salt.Length < SaltSize || nonce.Length != NonceSize || tag.Length != TagSize)
                throw new VaultFormatException("Invalid vault header.");

            return new VaultEnvelope(new VaultKdfParameters(model.Kdf.Algorithm, model.Kdf.Iterations, salt), nonce, tag, ciphertext);
        }
        catch (FormatException exception)
        {
            throw new VaultFormatException("Invalid vault header.", exception);
        }
    }

    /// <summary>
    /// Decrypt and authenticate. Returns false if the key is wrong or the file was modified.
    /// </summary>
    public static bool TryOpen(VaultEnvelope envelope, byte[] key, out byte[] plaintext)
    {
        plaintext = new byte[envelope.Ciphertext.Length];
        try
        {
            using AesGcm aes = new(key, TagSize);
            aes.Decrypt(envelope.Nonce, envelope.Ciphertext, envelope.Tag, plaintext, AssociatedData(envelope.Kdf));
            return true;
        }
        catch (AuthenticationTagMismatchException)
        {
            CryptographicOperations.ZeroMemory(plaintext);
            plaintext = [];
            return false;
        }
    }

    /// <summary>
    /// Header fields bound to the ciphertext: changing any of them makes decryption fail.
    /// </summary>
    private static byte[] AssociatedData(VaultKdfParameters kdf) =>
        Encoding.UTF8.GetBytes($"{FormatName}|{FormatVersion}|{kdf.Algorithm}|{kdf.Iterations}|{Convert.ToBase64String(kdf.Salt)}|{CipherAlgorithm}");
}

/// <summary>
/// The file is not a valid vault file (wrong format, unsupported version, corrupted header).
/// </summary>
public class VaultFormatException : Exception
{
    public VaultFormatException(string message) : base(message) { }
    public VaultFormatException(string message, Exception inner) : base(message, inner) { }
}
