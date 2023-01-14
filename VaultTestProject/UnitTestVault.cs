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
            MyVault.Instance.Erase();
            Assert.IsFalse(MyVault.Instance.IsInitialized());
            MyVault.Instance.Initialize(Assembly.GetExecutingAssembly().Location);
            Assert.IsTrue(MyVault.Instance.IsInitialized());
            MyVault.Instance.Erase();
        }

        [TestMethod("Check directory")]
        public void TestMethodDirectoryChecker()
        {
            MyVault.Instance.Erase();
            Assert.IsFalse(MyVault.Instance.IsInitialized());
            MyVault.Instance.Initialize(Assembly.GetExecutingAssembly().Location);
            Assert.IsTrue(MyVault.Instance.IsInitialized());
            Assert.IsTrue(MyVault.Instance.CheckDirectory(Assembly.GetExecutingAssembly().Location));
            MyVault.Instance.Erase();
        }

        [TestMethod("Test save and load password")]
        public void TestMethodMyPasswordManager()
        {
            MyVault.Instance.Erase();
            Assert.IsFalse(MyVault.Instance.IsInitialized());
            MyVault.Instance.Initialize(Assembly.GetExecutingAssembly().Location);
            Assert.IsTrue(MyVault.Instance.IsInitialized());
            Assert.IsTrue(MyVault.Instance.CheckDirectory(Assembly.GetExecutingAssembly().Location));
            MyVault.Instance.Vault.Add(new VaultCore.Models.MyPassword()
            {
                Data = "Data",
                Keyword = "Keyword",
                Password = "Password",
                UserName = "UserName"
            });
            MyVault.Instance.Save();
            MyVault.Instance.Vault.Clear();
            MyVault.Instance.Load();
            Assert.IsTrue(MyVault.Instance.Vault.Count > 0);
            Assert.IsTrue(MyVault.Instance.Vault[0].Data == "Data");
            Assert.IsTrue(MyVault.Instance.Vault[0].Keyword == "Keyword");
            Assert.IsTrue(MyVault.Instance.Vault[0].Password == "Password");
            Assert.IsTrue(MyVault.Instance.Vault[0].UserName == "UserName");
            MyVault.Instance.Erase();
        }
    }
}