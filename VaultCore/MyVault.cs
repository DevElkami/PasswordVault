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

        #region Singleton
        static private MyVault? instance = null;
        static private Object objectlock = new();

        /// <summary>
        /// Easy access to High level API
        /// </summary>
        public static MyVault Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objectlock)
                    {
                        if (instance == null)
                            instance = new MyVault();
                    }
                }
                return instance;
            }
        }

        private MyVault()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// All password
        /// </summary>
        public List<MyPassword> Vault = new List<MyPassword>();
        #endregion

        #region Read / write in encrypt file
        /// <summary>
        /// Save all password in a file
        /// </summary>
        public void Save()
        {
            List<String> data = new List<String>();
            foreach (MyPassword myPassword in Vault)
                data.Add(Security.Instance.Encrypt(myPassword.ToStr()));

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
                    MyPassword? myPassword = MyPassword.From(Security.Instance.Decrypt(data));
                    if (myPassword != null)
                        Vault.Add(myPassword);
                }
            }
        }
        #endregion
    }
}
