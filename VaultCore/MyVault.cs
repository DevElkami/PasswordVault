using System.Reflection;
using System.Text;
using VaultCore.Models;

namespace VaultCore
{
    /// <summary>
    /// High level API
    /// </summary>
    public class MyVault
    {
        /// <summary>
        /// Why this name ? Just deceive thief password
        /// </summary>
        private const String FILE_NAME = "SimpleDb.sqlite";

        /// <summary>
        /// Vault key (encrypted)
        /// </summary>
        private String vaultKey = "mykey";

        #region Properties
        /// <summary>
        /// All password
        /// </summary>
        public List<MyPassword> Vault = new();
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
        public void ImportOldData(String oldKey)
        {
            Vault.Clear();

            Mdp.OldVaultDecryptor oldVaultDecryptor = new();
            oldVaultDecryptor.Decrypt(oldKey);

            foreach (Mdp.OldPassword oldPassword in oldVaultDecryptor.Vault)
            {
                Vault.Add(new MyPassword()
                {
                    Data = oldPassword.Data,
                    Keyword = oldPassword.Keyword,
                    UserName = oldPassword.UserName,
                    Password = oldPassword.Password
                });
            }

            Save();
        }
        #endregion

        #region Read / write in encrypt file
        /// <summary>
        /// Save all password in a file
        /// </summary>
        public void Save()
        {
            List<String> data = new();
            foreach (MyPassword myPassword in Vault)
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
                Vault.Clear();

                foreach (String data in File.ReadAllLines(pathFile))
                {
                    MyPassword? myPassword = MyPassword.From(Security.Decrypt(vaultKey, data));
                    if (myPassword != null)
                        Vault.Add(myPassword);
                }
            }
        }
        #endregion
    }
}
