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
            myVault.Initialize(Assembly.GetExecutingAssembly().Location);
            Assert.IsTrue(myVault.IsInitialized());
            myVault.Erase();
        }

        [TestMethod("Check directory")]
        public void TestMethodDirectoryChecker()
        {
            MyVault myVault = new();
            myVault.Erase();
            Assert.IsFalse(myVault.IsInitialized());
            myVault.Initialize(Assembly.GetExecutingAssembly().Location);
            Assert.IsTrue(myVault.IsInitialized());
            Assert.IsTrue(myVault.CheckDirectory(Assembly.GetExecutingAssembly().Location));
            myVault.Erase();
        }

        [TestMethod("Test save and load password")]
        public void TestMethodMyPasswordManager()
        {
            MyVault myVault = new();
            myVault.Erase();
            Assert.IsFalse(myVault.IsInitialized());
            myVault.Initialize(Assembly.GetExecutingAssembly().Location);
            Assert.IsTrue(myVault.IsInitialized());
            Assert.IsTrue(myVault.CheckDirectory(Assembly.GetExecutingAssembly().Location));
            myVault.Vault.Add(new VaultCore.Models.MyPassword()
            {
                Data = "Data",
                Keyword = "Keyword",
                Password = "Password",
                UserName = "UserName"
            });
            myVault.Save();
            myVault.Vault.Clear();
            myVault.Load();
            Assert.IsTrue(myVault.Vault.Count > 0);
            Assert.IsTrue(myVault.Vault[0].Data == "Data");
            Assert.IsTrue(myVault.Vault[0].Keyword == "Keyword");
            Assert.IsTrue(myVault.Vault[0].Password == "Password");
            Assert.IsTrue(myVault.Vault[0].UserName == "UserName");
            myVault.Erase();
        }
    }
}