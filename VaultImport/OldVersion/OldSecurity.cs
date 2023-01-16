using System.Security.Cryptography;
using System.Text;

namespace Mdp
{
    /// <summary>
    /// Manage encrypted data
    /// </summary>
    public class OldSecurity
    {
        #region Private data 
        private String secureMdp = String.Empty;
        #endregion

        public OldSecurity(String _secureMdp)
        {
            this.secureMdp = GetMD5(_secureMdp);
        }

        #region Public functions          

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
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(secureMdp);

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
}
