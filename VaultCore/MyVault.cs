using CsvHelper;
using CsvHelper.Configuration;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using VaultCore.Models;

namespace VaultCore;

/// <summary>
/// High level API
/// </summary>
public class MyVault : BindingList<MyPassword>
{
    #region Private data        
    /// <summary>
    /// Vault key (encrypted)
    /// </summary>
    private String vaultKey = "mykey";
    #endregion

    #region Path management
    private String GetVaultPathKey()
    {
#pragma warning disable CS8604 // Existence possible d'un argument de référence null.
#if !DEBUG
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{nameof(VaultCore)}.key");
#else
        return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{nameof(VaultCore)}.key");
#endif
#pragma warning restore CS8604 // Existence possible d'un argument de référence null.
    }

    private String GetVaultPathData()
    {
#pragma warning disable CS8604 // Existence possible d'un argument de référence null.
#if !DEBUG
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SimpleDb.sqlite");
#else
        return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SimpleDb.sqlite");
#endif
#pragma warning restore CS8604 // Existence possible d'un argument de référence null.            
    }
    #endregion

    public MyVault()
    {
        AllowNew = true;
        AllowRemove = true;
        AllowEdit = true;

        RaiseListChangedEvents = true;
    }

    #region Vault access management
    /// <summary>
    /// Add password in vault if not already exist
    /// </summary>
    /// <param name="myPassword"></param>
    public new void Add(MyPassword myPassword)
    {
        if (!Contains(myPassword))
            base.Add(myPassword);
    }

    /// <summary>
    /// Check if a password are already in vault
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public new bool Contains(MyPassword item)
    {
        return this.Any(x => x.ToStr() == item.ToStr());
    }

    /// <summary>
    /// Check vault key
    /// </summary>
    /// <returns></returns>
    public bool CheckVaultKey(String key)
    {
        if (!IsInitialized())
            return false;

        Thread.Sleep(500);
        vaultKey = Security.GetHash(key);
        return Security.Encrypt(vaultKey, Security.GetHash(nameof(VaultCore) + nameof(Security))) == File.ReadAllText(GetVaultPathKey());
    }

    /// <summary>
    /// Check if vault is initialized
    /// </summary>
    /// <returns></returns>
    public bool IsInitialized()
    {
        return File.Exists(GetVaultPathKey());
    }

    /// <summary>
    /// Initialize vault file
    /// </summary>
    /// <param name="key">New vault key</param>
    /// <returns>True if FILE_NAME already exist: we have a old file version</returns>
    public bool Initialize(String key)
    {
        vaultKey = Security.GetHash(key);
        File.WriteAllText(GetVaultPathKey(), Security.Encrypt(vaultKey, Security.GetHash(nameof(VaultCore) + nameof(Security))));

        return File.Exists(GetVaultPathData());
    }

    /// <summary>
    /// Erase vault file
    /// </summary>
    public void Erase()
    {
        File.Delete(GetVaultPathKey());
    }

    /// <summary>
    /// Get datas from Firefox and thunderbird.        
    /// </summary>
    public static List<MyPassword> GetMyPasswordsFromBrowsers(String fullExportFilePath)
    {
        if (!String.IsNullOrEmpty(fullExportFilePath) && File.Exists(fullExportFilePath))
        {
            using (var reader = new StreamReader(fullExportFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<MyPasswordMap>();
                return new List<MyPassword>(csv.GetRecords<MyPassword>());
            }
        }

        return new List<MyPassword>();
    }
    #endregion

    #region Read / write in encrypt file
    /// <summary>
    /// Save all password in a file
    /// </summary>
    public void Save()
    {
        List<String> data = new();
        foreach (MyPassword myPassword in this)
            data.Add(Security.Encrypt(vaultKey, myPassword.ToStr()));

        // Pour éviter les doublons
        data = new List<String>(data.GroupBy(x => x.ToString()).Select(x => x.First()));
        File.WriteAllLines(GetVaultPathData(), data);
    }

    /// <summary>
    /// Load file and fill passwords collection
    /// </summary>
    public void Load()
    {
        if (File.Exists(GetVaultPathData()))
        {
            Clear();

            foreach (String data in File.ReadAllLines(GetVaultPathData()))
            {
                MyPassword? myPassword = MyPassword.From(Security.Decrypt(vaultKey, data));
                if (myPassword != null)
                    Add(myPassword);
            }
        }
    }
    #endregion
}

/// <summary>
/// Class mapper (csv to MyPassword object)
/// </summary>
public sealed class MyPasswordMap : ClassMap<MyPassword>
{
    public MyPasswordMap()
    {
        Map(m => m.Data).Name("url");
        Map(m => m.UserName).Name("username");
        Map(m => m.Password).Name("password");
        Map(m => m.Keyword).Ignore();
    }
}
