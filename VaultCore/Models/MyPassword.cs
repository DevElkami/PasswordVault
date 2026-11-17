using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VaultCore.Crypto;

namespace VaultCore.Models;

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
    // Setters never store null: old vault files can hold "Keyword": null, and search, duplicates
    // and merge all read these fields as plain strings.
    /// <summary>
    /// Url or anything else
    /// </summary>
    public String Data
    {
        get { return data; }
        set
        {
            value ??= String.Empty;
            if (data != value)
            {
                data = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Title));
                NotifyPropertyChanged(nameof(KeywordLine));
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
            value ??= String.Empty;
            if (username != value)
            {
                username = value;
                NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Password (clear text in memory, encrypted with the whole vault on disk)
    /// </summary>
    public String Password
    {
        get { return password; }
        set
        {
            value ??= String.Empty;
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
            value ??= String.Empty;
            if (keyword != value)
            {
                keyword = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Title));
                NotifyPropertyChanged(nameof(KeywordLine));
            }
        }
    }

    /// <summary>
    /// Stable identifier, used to merge two versions of the vault (see VaultMerge).
    /// Assigned by the vault when empty.
    /// </summary>
    public String Id { get; set => field = value ?? String.Empty; } = String.Empty;

    /// <summary>
    /// Last change (UTC). The newest version wins when two versions of the vault are merged.
    /// </summary>
    public DateTimeOffset Modified { get; set; }

    /// <summary>
    /// Display name: the site, or the keyword when there is none. Not stored.
    /// </summary>
    [JsonIgnore]
    public String Title => !String.IsNullOrWhiteSpace(Data) ? Data : Keyword;

    /// <summary>
    /// Keyword shown under the site and the login; empty when the keyword is already the title. Not stored.
    /// </summary>
    [JsonIgnore]
    public String KeywordLine => !String.IsNullOrWhiteSpace(Data) ? Keyword : String.Empty;
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
        return JsonSerializer.Serialize(this, VaultJsonContext.Default.MyPassword);
    }

    /// <summary>
    /// Raw data to MyPassword object
    /// </summary>
    /// <param name="data">JSON data</param>
    /// <returns>MyPassword</returns>
    public static MyPassword? From(String data)
    {
        return JsonSerializer.Deserialize(data, VaultJsonContext.Default.MyPassword);
    }
    #endregion

    #region Identity
    /// <summary>
    /// Same site, user, password and keyword (ignores <see cref="Id"/> and <see cref="Modified"/>).
    /// </summary>
    public bool HasSameContent(MyPassword other) =>
        Data == other.Data && UserName == other.UserName && Password == other.Password && Keyword == other.Keyword;

    internal string ContentKey() => String.Join('\u001F', Data, UserName, Password, Keyword);

    /// <summary>
    /// Id derived from the content, for entries saved before ids existed: the same old entry
    /// always gets the same id, so merging does not duplicate it.
    /// </summary>
    internal static string ContentId(MyPassword entry)
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(entry.ContentKey()));
        return Convert.ToHexStringLower(hash.AsSpan(0, 16));
    }
    #endregion

    public override string ToString()
    {
        return ToStr();
    }
}
