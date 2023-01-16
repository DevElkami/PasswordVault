using System.Reflection;

namespace Mdp
{
    /// <summary>
    /// High level API
    /// </summary>
    public class OldVaultDecryptor
    {
        #region Properties
        /// <summary>
        /// All password
        /// </summary>
        public List<OldPassword> Vault = new List<OldPassword>();
        #endregion

        #region Decrypt file
        /// <summary>
        /// Decrypt file and fill passwords collection
        /// </summary>
        public void Decrypt(String secureMdp)
        {
            OldSecurity security = new OldSecurity(secureMdp);
#pragma warning disable CS8604 // Existence possible d'un argument de référence null.
            String pathFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SimpleDb.sqlite");
#pragma warning restore CS8604 // Existence possible d'un argument de référence null.
            if (File.Exists(pathFile))
            {
                Vault.Clear();

                foreach (String data in File.ReadAllLines(pathFile))
                    Vault.Add(OldPassword.From(security.Decrypt(data)));
            }
        }
        #endregion
    }
}
