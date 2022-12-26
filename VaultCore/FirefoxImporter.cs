// Very smart approach: Interop with Firefox dll (mozglue.dll and nss3.dll)
// Thx: github.com/LimerBoy/FireFox-Thief
using Firefox.Reader;
using Firefox.Helpers;
using static Firefox.Helpers.Common;
using VaultCore.Models;

namespace VaultCore
{
    /// <summary>
    /// Class helper to retrieve data in firefox password vault
    /// </summary>
    public class FirefoxImporter
    {
        public static List<MyPassword> Import()
        {
            List<MyPassword> lp = new List<MyPassword>();
            foreach (string browser in Profile.GetMozillaBrowsers())
            {
                foreach (Password account in Passwords.Get(browser))
                {
                    if (string.IsNullOrEmpty(account.sUsername))
                        continue;
                    lp.Add(new MyPassword { Data = account.sUrl, UserName = account.sUsername, Password = account.sPassword.ToSecureString() });
                }
            }

            return lp;
        }
    }
}
