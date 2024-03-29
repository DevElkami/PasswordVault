using System.Reflection;
using VaultCore;

namespace VaultTestProject
{
    [TestClass]
    public class UnitTestVault
    {
        [TestMethod("Initialize")]
        public void TestMethodInitialize()
        {
            MyVault myVault = new();
            myVault.Erase();
            Assert.IsFalse(myVault.IsInitialized());
            myVault.Initialize("mypassword");
            Assert.IsTrue(myVault.IsInitialized());
            myVault.Erase();
        }

        [TestMethod("Check key")]
        public void TestMethodKeyChecker()
        {
            MyVault myVault = new();
            myVault.Erase();
            Assert.IsFalse(myVault.IsInitialized());
            myVault.Initialize("mypassword");
            Assert.IsTrue(myVault.IsInitialized());
            Assert.IsTrue(myVault.CheckVaultKey("mypassword"));
            myVault.Erase();
        }

        [TestMethod("Test save and load password")]
        public void TestMethodMyPasswordManager()
        {
            MyVault myVault = new();
            myVault.Erase();
            Assert.IsFalse(myVault.IsInitialized());
            myVault.Initialize("mypassword");
            Assert.IsTrue(myVault.IsInitialized());
            Assert.IsTrue(myVault.CheckVaultKey("mypassword"));
            myVault.Add(new VaultCore.Models.MyPassword()
            {
                Data = "Data",
                Keyword = "Keyword",
                Password = "Password",
                UserName = "UserName"
            });
            myVault.Add(new VaultCore.Models.MyPassword()
            {
                Data = "Data",
                Keyword = "Keyword",
                Password = "Password",
                UserName = "UserName"
            });
            Assert.IsTrue(myVault.Count == 1);
            myVault.Save();
            myVault.Clear();
            myVault.Load();
            Assert.IsTrue(myVault.Count > 0);
            Assert.IsTrue(myVault[0]?.Data == "Data");
            Assert.IsTrue(myVault[0]?.Keyword == "Keyword");
            Assert.IsTrue(myVault[0]?.Password == "Password");
            Assert.IsTrue(myVault[0]?.UserName == "UserName");
            myVault.Erase();
        }

        [TestMethod("Browser import")]
        public void TestMethodBrowserImport()
        {
            MyVault myVault = new();
            Assert.IsTrue(MyVault.GetMyPasswordsFromBrowsers(String.Empty).Count <= 0);
            Assert.IsTrue(MyVault.GetMyPasswordsFromBrowsers("").Count <= 0);
            Assert.IsTrue(MyVault.GetMyPasswordsFromBrowsers("path/dont/exist").Count <= 0);
        }
    }
}