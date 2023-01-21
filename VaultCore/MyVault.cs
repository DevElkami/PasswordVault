using System.Collections;
using System.Reflection;
using VaultCore.Models;

namespace VaultCore
{
    /// <summary>
    /// High level API
    /// </summary>
    public class MyVault : ICollection<MyPassword>
    {
        #region Private data
        /// <summary>
        /// Firefox data import helper
        /// </summary>
        private FirefoxImporter firefoxImporter = new();

        /// <summary>
        /// Why this name ? Just deceive thief password
        /// </summary>
        private const String FILE_NAME = "SimpleDb.sqlite";

        /// <summary>
        /// Vault key (encrypted)
        /// </summary>
        private String vaultKey = "mykey";

        /// <summary>
        /// All password
        /// </summary>
        private List<MyPassword> vault = new();
        #endregion

        #region Vault access management
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
            return Security.Encrypt(vaultKey, Security.GetHash(nameof(VaultCore) + nameof(Security))) == File.ReadAllText($"{nameof(VaultCore)}.key");
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
        /// Initialize vault file
        /// </summary>
        /// <param name="key">New vault key</param>
        /// <returns>True if FILE_NAME already exist: we have a old file version</returns>
        public bool Initialize(String key)
        {
            vaultKey = Security.GetHash(key);
            File.WriteAllText($"{nameof(VaultCore)}.key", Security.Encrypt(vaultKey, Security.GetHash(nameof(VaultCore) + nameof(Security))));

            return File.Exists(FILE_NAME);
        }

        /// <summary>
        /// Erase vault file
        /// </summary>
        public void Erase()
        {
            File.Delete($"{nameof(VaultCore)}.key");
        }

        /// <summary>
        /// Import old data (previous version 3.0.0.0)
        /// </summary>
        /// <param name="oldKey">Old vault key</param>
        public void ImportOldData(String oldKey)
        {
            vault.Clear();

            Mdp.OldVaultDecryptor oldVaultDecryptor = new();
            oldVaultDecryptor.Decrypt(oldKey);

            foreach (Mdp.OldPassword oldPassword in oldVaultDecryptor.Vault)
            {
                vault.Add(new MyPassword()
                {
                    Data = oldPassword.Data,
                    Keyword = oldPassword.Keyword,
                    UserName = oldPassword.UserName,
                    Password = oldPassword.Password
                });
            }

            Save();
        }

        /// <summary>
        /// Get datas from Firefox and thunderbird.        
        /// </summary>
        public List<MyPassword> GetMyPasswordsFromBrowsers()
        {
            return firefoxImporter.Import();
        }
        #endregion

        #region Read / write in encrypt file
        /// <summary>
        /// Save all password in a file
        /// </summary>
        public void Save()
        {
            List<String> data = new();
            foreach (MyPassword myPassword in vault)
                data.Add(Security.Encrypt(vaultKey, myPassword.ToStr()));

            // Pour éviter les doublons
            data = new List<String>(data.GroupBy(x => x.ToString()).Select(x => x.First()));

#pragma warning disable CS8604 // Existence possible d'un argument de référence null.
            File.WriteAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FILE_NAME), data);
#pragma warning restore CS8604 // Existence possible d'un argument de référence null.
        }

        /// <summary>
        /// Load file and fill passwords collection
        /// </summary>
        public void Load()
        {
#pragma warning disable CS8604 // Existence possible d'un argument de référence null.
            String pathFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FILE_NAME);
#pragma warning restore CS8604 // Existence possible d'un argument de référence null.
            if (File.Exists(pathFile))
            {
                vault.Clear();

                foreach (String data in File.ReadAllLines(pathFile))
                {
                    MyPassword? myPassword = MyPassword.From(Security.Decrypt(vaultKey, data));
                    if (myPassword != null)
                        vault.Add(myPassword);
                }
            }
        }
        #endregion

        #region ICollection
        public int Count => ((ICollection<MyPassword>)vault).Count;

        public bool IsReadOnly => ((ICollection<MyPassword>)vault).IsReadOnly;

        public void Add(MyPassword item)
        {
            if (!Contains(item))
                ((ICollection<MyPassword>)vault).Add(item);
        }

        public void Clear()
        {
            ((ICollection<MyPassword>)vault).Clear();
        }

        public bool Contains(MyPassword item)
        {
            return vault.Any(x => x.ToStr() == item.ToStr());
        }

        public void CopyTo(MyPassword[] array, int arrayIndex)
        {
            ((ICollection<MyPassword>)vault).CopyTo(array, arrayIndex);
        }

        public bool Remove(MyPassword item)
        {
            return ((ICollection<MyPassword>)vault).Remove(item);
        }

        public IEnumerator<MyPassword> GetEnumerator()
        {
            return ((IEnumerable<MyPassword>)vault).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)vault).GetEnumerator();
        }

        public MyPassword? this[int i]
        {
            get
            {
                if (i < vault.Count)
                    return vault[i];
                return null;
            }
            set
            {
                if ((i < vault.Count) && (value != null))
                    vault[i] = value;
            }
        }
        #endregion
    }
}
