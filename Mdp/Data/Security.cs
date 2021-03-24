using System;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Mdp
{
    /// <summary>
    /// Manage encrypted data
    /// </summary>
    public class Security
    {
        #region Private data 
        private SecureString secureMdp = new SecureString();
        #endregion

        #region Singleton
        static private Security instance = null;
        static private Object objectlock = new Object();

        /// <summary>
        /// Easy access to security access
        /// </summary>
        public static Security Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objectlock)
                    {
                        if (instance == null)
                        {
                            instance = new Security();

                            // MD5 hash of my favorite password
                            instance.secureMdp.AppendChar('9'); instance.secureMdp.AppendChar('b');
                            instance.secureMdp.AppendChar('9');
                            instance.secureMdp.AppendChar('6'); instance.secureMdp.AppendChar('8');
                            instance.secureMdp.AppendChar('2');
                            instance.secureMdp.AppendChar('b'); instance.secureMdp.AppendChar('6');
                            instance.secureMdp.AppendChar('9');
                            instance.secureMdp.AppendChar('0'); instance.secureMdp.AppendChar('5');
                            instance.secureMdp.AppendChar('0');
                            instance.secureMdp.AppendChar('a'); instance.secureMdp.AppendChar('9');
                            instance.secureMdp.AppendChar('4');
                            instance.secureMdp.AppendChar('7'); instance.secureMdp.AppendChar('d');
                            instance.secureMdp.AppendChar('a');
                            instance.secureMdp.AppendChar('a'); instance.secureMdp.AppendChar('e');
                            instance.secureMdp.AppendChar('0');
                            instance.secureMdp.AppendChar('8'); instance.secureMdp.AppendChar('4');
                            instance.secureMdp.AppendChar('1');
                            instance.secureMdp.AppendChar('a'); instance.secureMdp.AppendChar('7');
                            instance.secureMdp.AppendChar('7');
                            instance.secureMdp.AppendChar('2'); instance.secureMdp.AppendChar('5');
                            instance.secureMdp.AppendChar('7');
                            instance.secureMdp.AppendChar('2'); instance.secureMdp.AppendChar('7');
                        }
                    }
                }
                return instance;
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
            return GetMD5(password) == secureMdp.ToStr();
        }

        /// <summary>
        /// Encrypt string with Triple DES (Data Encryption Standard)
        /// </summary>
        /// <param name="data">String</param>
        /// <returns>Encrypt</returns>
        public String Encrypt(String data)
        {
            using (TripleDESCryptoServiceProvider cryptoServiceProvider = GetCryptoService())
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
            using (TripleDESCryptoServiceProvider cryptoServiceProvider = GetCryptoService())
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
        /// Basic MD5 hash
        /// </summary>
        /// <param name="value">String</param>
        /// <returns>MD5 hash</returns>
        private String GetMD5(String value)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return GetMd5Hash(md5Hash, value);
            }
        }

        /// <summary>
        /// Low level MD5
        /// </summary>
        /// <param name="md5Hash">Type of algorithm</param>
        /// <param name="input">String</param>
        /// <returns>MD5 hash</returns>
        private String GetMd5Hash(MD5 md5Hash, String input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }

        /// <summary>
        /// Create crypto triple DES
        /// </summary>
        /// <returns>Triple DES provider</returns>
        private TripleDESCryptoServiceProvider GetCryptoService()
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(secureMdp.ToStr());

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray.Take(24).ToArray(),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            return tdes;
        }
        #endregion        
    }

    /// <summary>
    /// Secure string extender
    /// </summary>
    public static class SecurityExt
    {
        /// <summary>
        /// Convert string to secure string
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>Secure string</returns>
        public static SecureString ToSecureString(this String str)
        {
            SecureString secureString = new SecureString();
            foreach (char c in str)
                secureString.AppendChar(c);
            return secureString;
        }

        /// <summary>
        /// Convert secure string to string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String ToStr(this SecureString str)
        {
            return new System.Net.NetworkCredential(String.Empty, str).Password;
        }
    }
}
