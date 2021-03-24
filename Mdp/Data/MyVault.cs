using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Mdp
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
        static private MyVault instance = null;
        static private Object objectlock = new Object();

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

            File.WriteAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FILE_NAME), data);
        }

        /// <summary>
        /// Load file and fill passwords collection
        /// </summary>
        public void Load()
        {
            String pathFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FILE_NAME);
            if (File.Exists(pathFile))
            {
                Vault.Clear();

                foreach (String data in File.ReadAllLines(pathFile))
                    Vault.Add(MyPassword.From(Security.Instance.Decrypt(data)));
            }
        }
        #endregion
    }
}
