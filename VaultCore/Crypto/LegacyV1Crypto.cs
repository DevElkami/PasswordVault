using System.Security.Cryptography;
using System.Text;

namespace VaultCore.Crypto;

/// <summary>
/// Vault format v1 (versions 3.x and earlier). Read-only: kept only to migrate old vaults to v2.
/// <para>
/// Known weaknesses, which is why it is no longer used for writing: unsalted single SHA-512 as key
/// derivation, constant IV, no authentication, and a key file that acts as an offline password verifier.
/// </para>
/// </summary>
internal static class LegacyV1Crypto
{
    /// <summary>
    /// True if <paramref name="password"/> matches the v1 key file content.
    /// </summary>
    public static bool CheckPassword(string password, string keyFileContent)
    {
        string vaultKey = GetHash(password);
        string expected = Encrypt(vaultKey, GetHash(nameof(VaultCore) + "Security"));
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(expected),
            Encoding.UTF8.GetBytes(keyFileContent.Trim()));
    }

    /// <summary>
    /// Decrypt every line of a v1 data file.
    /// </summary>
    public static IEnumerable<string> DecryptDataLines(string password, IEnumerable<string> lines)
    {
        string vaultKey = GetHash(password);
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
                yield return Decrypt(vaultKey, line);
        }
    }

    public static string Encrypt(string key, string data)
    {
        using Aes aes = GetCryptoService(key);
        using MemoryStream ms = new();
        using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            cs.Write(bytes, 0, bytes.Length);
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string key, string data)
    {
        using Aes aes = GetCryptoService(key);
        using MemoryStream ms = new();
        using (CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
        {
            byte[] bytes = Convert.FromBase64String(data);
            cs.Write(bytes, 0, bytes.Length);
        }
        return Encoding.UTF8.GetString(ms.ToArray());
    }

    public static string GetHash(string value) =>
        Convert.ToBase64String(SHA512.HashData(Encoding.UTF8.GetBytes(value)));

    public static Aes GetCryptoService(string key)
    {
        Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key.Take(aes.KeySize / 8).ToArray());
        aes.IV = Encoding.UTF8.GetBytes(GetHash("GetCryptoService")).Take(aes.BlockSize / 8).ToArray();
        return aes;
    }
}
