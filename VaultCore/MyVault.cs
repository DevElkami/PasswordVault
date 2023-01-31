using System.ComponentModel;
using System.Reflection;
using VaultCore.Models;

namespace VaultCore
{
    /// <summary>
    /// High level API
    /// </summary>
    public class MyVault : BindingList<MyPassword>
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
            foreach (MyPassword myPassword in this)
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
                Clear();

                foreach (String data in File.ReadAllLines(pathFile))
                {
                    MyPassword? myPassword = MyPassword.From(Security.Decrypt(vaultKey, data));
                    if (myPassword != null)
                        Add(myPassword);
                }
            }
        }
        #endregion        
    }
}
