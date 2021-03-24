using System;
using System.Security;

namespace Mdp
{
    /// <summary>
    /// Password data
    /// </summary>
    public class MyPassword
    {
        /// <summary>
        /// Url, or anything else
        /// </summary>
        public String Data { get; set; } = String.Empty;

        /// <summary>
        /// User name
        /// </summary>
        public String UserName { get; set; } = String.Empty;

        /// <summary>
        /// Encrypted password
        /// </summary>
        public SecureString Password { get; set; } = new SecureString();

        /// <summary>
        /// Decrypt password (used by gui binding)
        /// </summary>
        public String DecryptPwd { get { return Password.ToStr(); } }

        /// <summary>
        /// Keyword to help to find data
        /// </summary>
        public String Keyword { get; set; } = String.Empty;

        /// <summary>
        /// Used by gui binding
        /// </summary>
        /// <returns></returns>
        public String ToStr()
        {
            return Data + Environment.NewLine + UserName + Environment.NewLine + Password.ToStr() + Environment.NewLine + Keyword;
        }

        /// <summary>
        /// Raw data to MyPassword object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MyPassword From(String data)
        {
            String[] values = data.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
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
        public override String ToString()
        {
            if (String.IsNullOrEmpty(Keyword))
                return "Login: " + UserName + " Mdp: " + Password + " Data: " + Data;

            if (String.IsNullOrEmpty(Data))
                return "Login: " + UserName + " Mdp: " + Password + " Keyword: " + Keyword;

            return "Login: " + UserName + " Mdp: " + Password + " Keyword: " + Keyword + " Data: " + Data;
        }
    }
}
