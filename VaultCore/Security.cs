using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace VaultCore
{
    /// <summary>
    /// Toolkit to Encrypted data
    /// </summary>
    public class Security
    {
        /// <summary>
        /// Encrypt string with symmetric algorithm
        /// </summary>
        /// <param name="data">String</param>
        /// <returns>Encrypt</returns>
        public static String Encrypt(String key, String data)
        {
            using (Aes cryptoServiceProvider = GetCryptoService(key))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, cryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(data);
                        cs.Write(toEncryptArray, 0, toEncryptArray.Length);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypt string
        /// </summary>
        /// <param name="data">Encrypted string</param>
        /// <returns>String</returns>
        public static String Decrypt(String key, String data)
        {
            using (Aes cryptoServiceProvider = GetCryptoService(key))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, cryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        byte[] toEncryptArray = Convert.FromBase64String(data);
                        cs.Write(toEncryptArray, 0, toEncryptArray.Length);
                    }

                    return UTF8Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Get hash of string
        /// </summary>
        /// <param name="value">String to hash</param>
        /// <returns>Hash</returns>
        public static String GetHash(String value)
        {
            Byte[] encryptedBytes = Encoding.UTF8.GetBytes(nameof(GetHash));
            using (SHA512 hashTool = SHA512.Create())
            {
                encryptedBytes = hashTool.ComputeHash(Encoding.UTF8.GetBytes(value));
                hashTool.Clear();
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Create crypto AES
        /// </summary>
        /// <returns>AES provider</returns>
        public static Aes GetCryptoService(string key)
        {
            Aes aes = Aes.Create();
            aes.Key = UTF8Encoding.UTF8.GetBytes(key.Take(aes.KeySize / 8).ToArray());
            aes.IV = UTF8Encoding.UTF8.GetBytes(GetHash(nameof(GetCryptoService)).ToString()).Take(aes.BlockSize / 8).ToArray();

            return aes;
        }
    }
}
