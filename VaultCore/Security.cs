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
            using (Aes cryptoServiceProvider = GetCryptoService())
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
            using (Aes cryptoServiceProvider = GetCryptoService())
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
        #endregion

        #region Private function
        /// <summary>
        /// Get hash of string
        /// </summary>
        /// <param name="value">String to hash</param>
        /// <returns>Hash</returns>
        private String GetHash(String value)
        {
            Byte[] EncryptedBytes = Encoding.UTF8.GetBytes(nameof(GetHash));
            using (SHA512 hashTool = SHA512.Create())
            {
                EncryptedBytes = hashTool.ComputeHash(Encoding.UTF8.GetBytes(value));
                hashTool.Clear();
            }
            return Convert.ToBase64String(EncryptedBytes);
        }

        /*byte[] EncryptStringToBytes_Aes(string plainText)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
           
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = GetCryptoService())
            {               
                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        string DecryptStringFromBytes_Aes(byte[] cipherText)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");           

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = GetCryptoService())
            {                
                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }*/

        /// <summary>
        /// Create crypto AES
        /// </summary>
        /// <returns>AES provider</returns>
        private Aes GetCryptoService()
        {
            Aes aes = Aes.Create();
            aes.Key = UTF8Encoding.UTF8.GetBytes(secureMdp);
            aes.IV = UTF8Encoding.UTF8.GetBytes(GetHash(nameof(GetCryptoService)).ToString()).Take(aes.BlockSize / 8).ToArray();

            return aes;
        }
        #endregion
    }
}
