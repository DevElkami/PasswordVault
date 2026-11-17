using System.Text;
using System.Text.Json.Nodes;
using VaultCore.Crypto;

namespace VaultTestProject;

[TestClass]
public class UnitTestVaultCipher
{
    private static readonly byte[] Plaintext = Encoding.UTF8.GetBytes("""{"entries":[{"UserName":"me"}]}""");

    private static (byte[] key, VaultKdfParameters kdf) NewKey(string password = "correct horse")
    {
        VaultKdfParameters kdf = VaultCipher.NewKdfParameters(TestVaultDirectory.FastIterations);
        return (VaultCipher.DeriveKey(password, kdf), kdf);
    }

    [TestMethod(DisplayName = "Seal then open returns the plaintext")]
    public void RoundTrip()
    {
        (byte[] key, VaultKdfParameters kdf) = NewKey();
        VaultEnvelope envelope = VaultCipher.Parse(VaultCipher.Seal(Plaintext, key, kdf));

        Assert.IsTrue(VaultCipher.TryOpen(envelope, key, out byte[] result));
        CollectionAssert.AreEqual(Plaintext, result);
    }

    [TestMethod(DisplayName = "Wrong password cannot open")]
    public void WrongPassword()
    {
        (byte[] key, VaultKdfParameters kdf) = NewKey("right");
        VaultEnvelope envelope = VaultCipher.Parse(VaultCipher.Seal(Plaintext, key, kdf));

        byte[] wrongKey = VaultCipher.DeriveKey("wrong", envelope.Kdf);
        Assert.IsFalse(VaultCipher.TryOpen(envelope, wrongKey, out byte[] result));
        Assert.IsEmpty(result);
    }

    [TestMethod(DisplayName = "Same data encrypted twice gives different files (random nonce)")]
    public void NonDeterministic()
    {
        (byte[] key, VaultKdfParameters kdf) = NewKey();
        VaultEnvelope a = VaultCipher.Parse(VaultCipher.Seal(Plaintext, key, kdf));
        VaultEnvelope b = VaultCipher.Parse(VaultCipher.Seal(Plaintext, key, kdf));

        CollectionAssert.AreNotEqual(a.Nonce, b.Nonce);
        CollectionAssert.AreNotEqual(a.Ciphertext, b.Ciphertext);
    }

    [TestMethod(DisplayName = "Each new vault gets its own salt")]
    public void RandomSalt()
    {
        CollectionAssert.AreNotEqual(VaultCipher.NewKdfParameters().Salt, VaultCipher.NewKdfParameters().Salt);
        Assert.AreEqual(VaultCipher.DefaultIterations, VaultCipher.NewKdfParameters().Iterations);
    }

    [TestMethod(DisplayName = "Modified ciphertext is detected")]
    public void TamperedPayload()
    {
        (byte[] key, VaultKdfParameters kdf) = NewKey();
        VaultEnvelope envelope = VaultCipher.Parse(VaultCipher.Seal(Plaintext, key, kdf));
        envelope.Ciphertext[0] ^= 0x01;

        Assert.IsFalse(VaultCipher.TryOpen(envelope, key, out _));
    }

    [TestMethod(DisplayName = "Modified header is detected (authenticated as associated data)")]
    public void TamperedHeader()
    {
        (byte[] key, VaultKdfParameters kdf) = NewKey();
        JsonNode file = JsonNode.Parse(VaultCipher.Seal(Plaintext, key, kdf))!;
        file["kdf"]!["iterations"] = TestVaultDirectory.FastIterations + 1;

        VaultEnvelope envelope = VaultCipher.Parse(Encoding.UTF8.GetBytes(file.ToJsonString()));

        // Even with the right key, the changed header must make decryption fail.
        Assert.IsFalse(VaultCipher.TryOpen(envelope, key, out _));
    }

    [TestMethod(DisplayName = "Non-vault content is rejected")]
    [DataRow("")]
    [DataRow("hello")]
    [DataRow("""{"format":"Other","version":2}""")]
    [DataRow("""{"format":"PasswordVault","version":99}""")]
    [DataRow("""{"format":"PasswordVault","version":2,"kdf":{"algorithm":"PBKDF2-HMAC-SHA256","iterations":2000000000,"salt":"AAAAAAAAAAAAAAAAAAAAAA=="}}""")]
    [DataRow("dGhpcyBpcyBhIHYxIGxpbmU=\nYW5vdGhlciB2MSBsaW5l")]
    public void RejectsInvalidFiles(string content)
    {
        Assert.ThrowsExactly<VaultFormatException>(() => VaultCipher.Parse(Encoding.UTF8.GetBytes(content)));
        Assert.IsFalse(VaultCipher.IsVaultFile(Encoding.UTF8.GetBytes(content)));
    }

    [TestMethod(DisplayName = "Legacy v1 crypto still reads what 3.x wrote")]
    public void LegacyCompatibility()
    {
        using TestVaultDirectory dir = new();
        string keyPath = Path.Combine(dir.Path, "k");
        string dataPath = Path.Combine(dir.Path, "d");
        OriginalV1Writer.WriteVault(keyPath, dataPath, "oldpassword", [new() { UserName = "bob" }]);

        Assert.IsTrue(LegacyV1Crypto.CheckPassword("oldpassword", File.ReadAllText(keyPath)));
        Assert.IsFalse(LegacyV1Crypto.CheckPassword("oldpassworD", File.ReadAllText(keyPath)));
        StringAssert.Contains(LegacyV1Crypto.DecryptDataLines("oldpassword", File.ReadAllLines(dataPath)).Single(), "\"bob\"");
    }
}
