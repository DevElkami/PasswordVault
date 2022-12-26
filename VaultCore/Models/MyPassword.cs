using System;
using System.Security;

namespace VaultCore.Models
{
    /// <summary>
    /// Password data
    /// </summary>
    public class MyPassword
    {
        /// <summary>
        /// Url, or anything else
        /// </summary>
        public string Data { get; set; } = string.Empty;

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Encrypted password
        /// </summary>
        public SecureString Password { get; set; } = new SecureString();

        /// <summary>
        /// Decrypt password (used by gui binding)
        /// </summary>
        public string DecryptPwd { get { return Password.ToStr(); } }

        /// <summary>
        /// Keyword to help to find data
        /// </summary>
        public string Keyword { get; set; } = string.Empty;

        /// <summary>
        /// Used by gui binding
        /// </summary>
        /// <returns></returns>
        public string ToStr()
        {
            return Data + Environment.NewLine + UserName + Environment.NewLine + Password.ToStr() + Environment.NewLine + Keyword;
        }

        /// <summary>
        /// Raw data to MyPassword object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MyPassword From(string data)
        {
            string[] values = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            MyPassword myPassword = new MyPassword()
            {
                Data = values[0],
                UserName = values[1],
                Password = values[2].ToSecureString(),
                Keyword = values[3]
            };

            return myPassword;
        }

        /// <summary>
        /// Override to string function (used by gui binding)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Keyword))
                return "Login: " + UserName + " Mdp: " + Password + " Data: " + Data;

            if (string.IsNullOrEmpty(Data))
                return "Login: " + UserName + " Mdp: " + Password + " Keyword: " + Keyword;

            return "Login: " + UserName + " Mdp: " + Password + " Keyword: " + Keyword + " Data: " + Data;
        }
    }
}
