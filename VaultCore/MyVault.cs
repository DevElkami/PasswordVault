using System.Reflection;
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
        private String directoryName = nameof(Security);        

        #region Properties
        /// <summary>
        /// All password
        /// </summary>
        public List<MyPassword> Vault = new();
        #endregion

        #region Vault access management        
        public bool CheckDirectory(String dir)
        {
            if (!IsInitialized())
                return false;

            Thread.Sleep(500);
            directoryName = dir;
            return Security.Encrypt(directoryName, Security.GetHash(nameof(VaultCore) + nameof(Security))) == File.ReadAllText($"{nameof(VaultCore)}.key");
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
        /// <param name="directory">Initial directory</param>
        public void Initialize(String directory)
        {
            File.WriteAllText($"{nameof(VaultCore)}.key", Security.Encrypt(directory, Security.GetHash(nameof(VaultCore) + nameof(Security))));
        }

        /// <summary>
        /// Erase vault file
        /// </summary>        
        public void Erase()
        {
            File.Delete($"{nameof(VaultCore)}.key");
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
                data.Add(Security.Encrypt(directoryName, myPassword.ToStr()));

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
                    MyPassword? myPassword = MyPassword.From(Security.Decrypt(directoryName, data));
                    if (myPassword != null)
                        Vault.Add(myPassword);
                }
            }
        }
        #endregion
    }
}
