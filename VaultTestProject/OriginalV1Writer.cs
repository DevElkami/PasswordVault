using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using VaultCore.Models;

namespace VaultTestProject;

/// <summary>
/// Verbatim copy of the 3.x (format v1) write path, used to build real v1 vaults for the migration tests.
/// Do not "fix" this code: it must keep producing exactly what old versions wrote to disk.
/// </summary>
internal static class OriginalV1Writer
{
    public static void WriteVault(string keyPath, string dataPath, string password, IEnumerable<MyPassword> entries)
    {
        string vaultKey = GetHash(password);
        File.WriteAllText(keyPath, Encrypt(vaultKey, GetHash("VaultCore" + "Security")));

        List<string> data = [];
        foreach (MyPassword myPassword in entries)
            data.Add(Encrypt(vaultKey, JsonSerializer.Serialize(myPassword, ReflectionOptions)));
        File.WriteAllLines(dataPath, data);
    }

    // The old code used reflection-based serialization with default options.
    private static readonly JsonSerializerOptions ReflectionOptions = new()
    {
        TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver(),
    };

    private static string Encrypt(string key, string data)
    {
        using Aes cryptoServiceProvider = GetCryptoService(key);
        using MemoryStream ms = new();
        using (CryptoStream cs = new(ms, cryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write))
        {
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(data);
            cs.Write(toEncryptArray, 0, toEncryptArray.Length);
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    private static string GetHash(string value)
    {
        byte[] encryptedBytes;
        using (SHA512 hashTool = SHA512.Create())
        {
            encryptedBytes = hashTool.ComputeHash(Encoding.UTF8.GetBytes(value));
            hashTool.Clear();
        }
        return Convert.ToBase64String(encryptedBytes);
    }

    private static Aes GetCryptoService(string key)
    {
        Aes aes = Aes.Create();
        aes.Key = UTF8Encoding.UTF8.GetBytes(key.Take(aes.KeySize / 8).ToArray());
        aes.IV = UTF8Encoding.UTF8.GetBytes(GetHash("GetCryptoService").ToString()).Take(aes.BlockSize / 8).ToArray();
        return aes;
    }
}
