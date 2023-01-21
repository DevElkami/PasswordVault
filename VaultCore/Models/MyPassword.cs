using System.Text.Json;

namespace VaultCore.Models
{
    /// <summary>
    /// Password data
    /// </summary>
    public class MyPassword
    {
        /// <summary>
        /// Url or anything else
        /// </summary>
        public String Data { get; set; } = String.Empty;

        /// <summary>
        /// User name or login
        /// </summary>
        public String UserName { get; set; } = String.Empty;

        /// <summary>
        /// Password (emcrypted)
        /// </summary>
        public String Password { get; set; } = String.Empty;

        /// <summary>
        /// Keyword to help to find data
        /// </summary>
        public String Keyword { get; set; } = String.Empty;

        /// <summary>
        /// MyPassword to raw data
        /// </summary>
        /// <returns>JSON string</returns>
        public string ToStr()
        {
            return JsonSerializer.Serialize(this);            
        }

        /// <summary>
        /// Raw data to MyPassword object
        /// </summary>
        /// <param name="data">JSON data</param>
        /// <returns>MyPassword</returns>
        public static MyPassword? From(String data)
        {
            return JsonSerializer.Deserialize<MyPassword>(data);            
        }

        public override string ToString()
        {
            return ToStr();
        }
    }
}
