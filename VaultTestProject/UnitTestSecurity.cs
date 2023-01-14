using System.Security.Cryptography;
using VaultCore;

namespace VaultTestProject
{
    [TestClass]
    public class UnitTestSecurity
    {
        [TestMethod("Hash")]
        public void TestMethodHash()
        {
            Assert.IsTrue(Security.GetHash("m").Length > 32);
            Assert.IsTrue(Security.GetHash("mykey").Length > 32);
            Assert.IsTrue(Security.GetHash("mykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykey").Length > 32);

            Assert.IsTrue(Security.GetHash("m").Length == Security.GetHash("mykey").Length);
            Assert.IsTrue(Security.GetHash("m").Length == Security.GetHash("mykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykey").Length);
        }

        [TestMethod("Create AES crypto service")]
        public void TestMethodCreateAES()
        {
            Assert.ThrowsException<CryptographicException>(() => Security.GetCryptoService("ToSmall"));
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
            Assert.ThrowsException<ArgumentNullException>(() => Security.GetCryptoService(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
            Assert.IsNotNull(Security.GetCryptoService("mykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykeymykey"));
        }

        [TestMethod("AES encrypt / decrypt")]
        public void TestMethodEncryptDecrypt()
        {
            string encryptData = Security.Encrypt(Security.GetHash("mykey"), "myencryptdata");
            Assert.IsTrue("myencryptdata" == Security.Decrypt(Security.GetHash("mykey"), encryptData));
        }
    }
}