using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace VaultCore
{
    /// <summary>
    /// Manage encrypted data
    /// </summary>
    public class Security
    {
        #region Private data 
        private String secureMdp = nameof(Security);
        #endregion

        #region Singleton
        static private Security? security = null;
        static private Object objectlock = new();

        /// <summary>
        /// Easy access to security access
        /// </summary>
        public static Security Instance
        {
            get
            {
                if (security == null)
                {
                    lock (objectlock)
                    {
                        if (security == null)
                            security = new Security();
                    }
                }
                return security;
            }
        }

        private Security()
        {
        }
        #endregion        

        #region Public functions
        /// <summary>
        /// Password checker
        /// </summary>
        /// <param name="password">Password</param>
        /// <returns>True if equal else false</returns>
        public bool CheckPassword(String password)
        {
            if (!IsInitialized())
                return false;

            Thread.Sleep(500);
            secureMdp = password;
            return Encrypt(GetHash(nameof(VaultCore) + nameof(Security))) == File.ReadAllText($"{nameof(VaultCore)}.key");
        }

        /// <summary>
        /// Check if vault is initialized
        /// </summary>
        /// <returns></returns>
        public bool IsInitialized()
        {
            return File.Exists($"{nameof(VaultCore)}.key");
        }

        /// <summary>
        /// Initialize password
        /// </summary>
        /// <param name="password"></param>
        public void Initialize(String password)
        {
            File.WriteAllText($"{nameof(VaultCore)}.key", Encrypt(GetHash(nameof(VaultCore) + nameof(Security))));
        }

        /// <summary>
        /// Encrypt string with symmetric algorithm
        /// </summary>
        /// <param name="data">String</param>
        /// <returns>Encrypt</returns>
        public String Encrypt(String data)
        {
            using (Aes cryptoServiceProvider = GetCryptoService(secureMdp))
            {
                using (ICryptoTransform cTransform = cryptoServiceProvider.CreateEncryptor())
                {
                    byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(data);
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    cryptoServiceProvider.Clear();

                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
        }

        /// <summary>
        /// Encrypt string with symmetric algorithm
        /// </summary>
        /// <param name="data">String</param>
        /// <returns>Encrypt</returns>
        public static String Encrypt(String key, String data)
        {
            using (Aes cryptoServiceProvider = GetCryptoService(key))
            {
                using (ICryptoTransform cTransform = cryptoServiceProvider.CreateEncryptor())
                {
                    byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(data);
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    cryptoServiceProvider.Clear();

                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
        }

        /// <summary>
        /// Decrypt string
        /// </summary>
        /// <param name="data">Encrypted string</param>
        /// <returns>String</returns>
        public String Decrypt(String data)
        {
            using (Aes cryptoServiceProvider = GetCryptoService(secureMdp))
            {
                using (ICryptoTransform cTransform = cryptoServiceProvider.CreateDecryptor())
                {
                    byte[] toEncryptArray = Convert.FromBase64String(data);
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    cryptoServiceProvider.Clear();

                    return UTF8Encoding.UTF8.GetString(resultArray);
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
                using (ICryptoTransform cTransform = cryptoServiceProvider.CreateDecryptor())
                {
                    byte[] toEncryptArray = Convert.FromBase64String(data);
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    cryptoServiceProvider.Clear();

                    return UTF8Encoding.UTF8.GetString(resultArray);
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
        #endregion
    }
}
