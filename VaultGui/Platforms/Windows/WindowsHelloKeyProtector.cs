using System.Runtime.InteropServices;
using System.Security.Cryptography;
using global::Windows.Security.Credentials;
using global::Windows.Security.Cryptography;
using global::Windows.Storage.Streams;
using VaultGui.Services;

namespace VaultGui.Platforms.Windows;

/// <summary>
/// Quick unlock with Windows Hello (fingerprint, face or Hello PIN).
/// <para>
/// A Windows Hello key pair is created for this app (private key in the TPM when there is one). It can only
/// sign after the user passes the Hello prompt. The signature of a random challenge is deterministic
/// (RSA PKCS#1 v1.5): it is turned into an AES-256-GCM key (HKDF) that encrypts the vault key.
/// Nothing that can decrypt is stored: getting the key back needs a new signature, so a new Hello prompt.
/// </para>
/// <para>
/// Unlike Android, enrolling a new fingerprint or face does not destroy the key, and the Hello PIN is accepted.
/// The key is lost (master password needed) when Windows Hello is reset.
/// </para>
/// </summary>
public sealed class WindowsHelloKeyProtector : IBiometricKeyProtector
{
    private const string KeyName = "PasswordVault.QuickUnlock";
    private const string FormatPrefix = "wh1";
    private const int ChallengeBytes = 32;
    private const int NonceBytes = 12;
    private const int TagBytes = 16;
    private static readonly byte[] KdfInfo = "PasswordVault.QuickUnlock.v1"u8.ToArray();

    public string Name => "Windows Hello";

    public async Task<bool> IsAvailableAsync()
    {
        try
        {
            return await KeyCredentialManager.IsSupportedAsync();
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<string?> ProtectAsync(byte[] key)
    {
        KeyCredentialRetrievalResult created = await WithPromptInFrontAsync(
            KeyCredentialManager.RequestCreateAsync(KeyName, KeyCredentialCreationOption.ReplaceExisting).AsTask());
        if (created.Status != KeyCredentialStatus.Success)
            return null;

        byte[] challenge = RandomNumberGenerator.GetBytes(ChallengeBytes);
        byte[]? wrappingKey = await SignAsync(created.Credential, challenge);
        if (wrappingKey is null)
        {
            Forget();
            return null;
        }

        try
        {
            byte[] nonce = RandomNumberGenerator.GetBytes(NonceBytes);
            byte[] sealedKey = new byte[key.Length + TagBytes];
            using AesGcm aes = new(wrappingKey, TagBytes);
            aes.Encrypt(nonce, key, sealedKey.AsSpan(0, key.Length), sealedKey.AsSpan(key.Length), challenge);
            return string.Join('.', FormatPrefix, Convert.ToBase64String(challenge), Convert.ToBase64String(nonce), Convert.ToBase64String(sealedKey));
        }
        finally
        {
            CryptographicOperations.ZeroMemory(wrappingKey);
        }
    }

    public async Task<byte[]?> UnprotectAsync(string protectedKey)
    {
        // Checked before any prompt: data of another format (older versions) can never be opened.
        byte[] challenge, nonce, sealedKey;
        try
        {
            string[] parts = protectedKey.Split('.');
            if (parts.Length != 4 || parts[0] != FormatPrefix)
                throw new FormatException();
            challenge = Convert.FromBase64String(parts[1]);
            nonce = Convert.FromBase64String(parts[2]);
            sealedKey = Convert.FromBase64String(parts[3]);
            if (nonce.Length != NonceBytes || sealedKey.Length <= TagBytes)
                throw new FormatException();
        }
        catch (FormatException exception)
        {
            throw new BiometricInvalidatedException("Unknown quick unlock data.", exception);
        }

        KeyCredentialRetrievalResult opened = await KeyCredentialManager.OpenAsync(KeyName);
        if (opened.Status == KeyCredentialStatus.NotFound)
            throw new BiometricInvalidatedException("The Windows Hello key no longer exists.");
        if (opened.Status != KeyCredentialStatus.Success)
            return null;

        byte[]? wrappingKey = await SignAsync(opened.Credential, challenge);
        if (wrappingKey is null)
            return null;

        int keyLength = sealedKey.Length - TagBytes;
        byte[] key = new byte[keyLength];
        try
        {
            using AesGcm aes = new(wrappingKey, TagBytes);
            aes.Decrypt(nonce, sealedKey.AsSpan(0, keyLength), sealedKey.AsSpan(keyLength), key, challenge);
            return key;
        }
        catch (CryptographicException exception)
        {
            // Windows Hello was reset and the key created again under the same name, or the data was altered.
            CryptographicOperations.ZeroMemory(key);
            throw new BiometricInvalidatedException("Quick unlock data does not match the Windows Hello key.", exception);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(wrappingKey);
        }
    }

    public void Forget() => _ = DeleteKeyAsync();

    private static async Task DeleteKeyAsync()
    {
        try
        {
            await KeyCredentialManager.DeleteAsync(KeyName);
        }
        catch (Exception)
        {
            // No such key (or no Windows Hello any more): nothing to delete.
        }
    }

    /// <summary>
    /// Windows Hello prompt, then the AES key derived from the signature of <paramref name="challenge"/>.
    /// Null if the user cancelled (or Hello is unavailable right now).
    /// </summary>
    private static async Task<byte[]?> SignAsync(KeyCredential credential, byte[] challenge)
    {
        IBuffer data = CryptographicBuffer.CreateFromByteArray(challenge);
        KeyCredentialOperationResult signed = await WithPromptInFrontAsync(credential.RequestSignAsync(data).AsTask());
        if (signed.Status != KeyCredentialStatus.Success)
            return null;

        CryptographicBuffer.CopyToByteArray(signed.Result, out byte[] signature);
        try
        {
            return HKDF.DeriveKey(HashAlgorithmName.SHA256, signature, 32, salt: challenge, info: KdfInfo);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(signature);
        }
    }

    /// <summary>
    /// For a desktop (unpackaged) app the Windows Hello dialog often opens behind the app window,
    /// only blinking in the task bar: bring it to the front as soon as it shows up.
    /// </summary>
    private static async Task<T> WithPromptInFrontAsync<T>(Task<T> operation)
    {
        for (int attempt = 0; attempt < 50 && !operation.IsCompleted; attempt++)
        {
            IntPtr prompt = FindWindow("Credential Dialog Xaml Host", null);
            if (prompt != IntPtr.Zero)
            {
                SetForegroundWindow(prompt);
                break;
            }

            await Task.WhenAny(operation, Task.Delay(100));
        }

        return await operation;
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindow(string className, string? windowName);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr window);
}
