using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace VaultCore.Models
{
    /// <summary>
    /// Password data
    /// </summary>
    public class MyPassword : INotifyPropertyChanged
    {
        #region Private data
        String data = String.Empty;
        String username = String.Empty;
        String password = String.Empty;
        String keyword = String.Empty;
        #endregion

        #region Properties        
        /// <summary>
        /// Url or anything else
        /// </summary>
        public String Data
        {
            get { return data; }
            set
            {
                if (data != value)
                {
                    data = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// User name or login
        /// </summary>
        public String UserName
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Password (encrypted)
        /// </summary>
        public String Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Keyword to help to find data
        /// </summary>
        public String Keyword
        {
            get { return keyword; }
            set
            {
                if (keyword != value)
                {
                    keyword = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region Notify property changed
        /// <summary>
        /// INotifyPropertyChanged interface
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Internal property changed access
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Json serializer
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
        #endregion

        public override string ToString()
        {
            return ToStr();
        }
    }
}
