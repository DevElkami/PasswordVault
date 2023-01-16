namespace Mdp
{
    /// <summary>
    /// Password data
    /// </summary>
    public class OldPassword
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
        public String Password { get; set; } = String.Empty;

        /// <summary>
        /// Keyword to help to find data
        /// </summary>
        public String Keyword { get; set; } = String.Empty;

        /// <summary>
        /// Raw data to MyPassword object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static OldPassword From(String data)
        {
            String[] values = data.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
            OldPassword myPassword = new OldPassword()
            {
                Data = values[0],
                UserName = values[1],
                Password = values[2],
                Keyword = values[3]
            };

            return myPassword;
        }
    }
}
