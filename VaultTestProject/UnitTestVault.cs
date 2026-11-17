using System.Security.Cryptography;
using System.Text;
using VaultCore;
using VaultCore.Import;
using VaultCore.Models;

namespace VaultTestProject;

[TestClass]
public class UnitTestVault
{
    private static MyPassword Sample(string user = "UserName") => new()
    {
        Data = "https://example.com",
        Keyword = "Keyword",
        Password = "S3cr3t-P@ss",
        UserName = user,
    };

    [TestMethod(DisplayName = "Create makes an unlocked vault")]
    public void Create()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();

        Assert.IsFalse(vault.IsInitialized);
        vault.Create("mypassword");
        Assert.IsTrue(vault.IsInitialized);
        Assert.IsTrue(vault.IsUnlocked);
        Assert.IsFalse(vault.NeedsMigration);
    }

    [TestMethod(DisplayName = "Create never overwrites an existing vault")]
    public void CreateTwice()
    {
        using TestVaultDirectory dir = new();
        dir.NewVault().Create("mypassword");

        Assert.ThrowsExactly<InvalidOperationException>(() => dir.NewVault().Create("other"));
    }

    [TestMethod(DisplayName = "Password check")]
    public void CheckKey()
    {
        using TestVaultDirectory dir = new();
        dir.NewVault().Create("mypassword");

        VaultService vault = dir.NewVault();
        Assert.IsFalse(vault.Unlock("wrongpassword"));
        Assert.IsFalse(vault.IsUnlocked);
        Assert.IsTrue(vault.Unlock("mypassword"));
        Assert.IsTrue(vault.IsUnlocked);
    }

    [TestMethod(DisplayName = "Save and load, duplicates removed")]
    public void SaveAndLoad()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create("mypassword");
        vault.Add(Sample());
        vault.Add(Sample());
        vault.Add(Sample("other"));
        Assert.HasCount(2, vault.Entries);
        vault.Save();

        VaultService reopened = dir.NewVault();
        Assert.IsTrue(reopened.Unlock("mypassword"));
        reopened.Reload();

        Assert.HasCount(2, reopened.Entries);
        MyPassword first = reopened.Entries.Single(x => x.UserName == "UserName");
        Assert.AreEqual("https://example.com", first.Data);
        Assert.AreEqual("Keyword", first.Keyword);
        Assert.AreEqual("S3cr3t-P@ss", first.Password);
    }

    [TestMethod(DisplayName = "Vault file contains no clear text")]
    public void NoClearText()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create("mypassword");
        vault.Add(Sample());
        vault.Save();

        string file = File.ReadAllText(dir.Options.VaultFilePath);
        Assert.DoesNotContain("S3cr3t-P@ss", file);
        Assert.DoesNotContain("example.com", file);
        StringAssert.Contains(file, "AES-256-GCM");
    }

    [TestMethod(DisplayName = "Tampered vault file is refused")]
    public void TamperedFile()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create("mypassword");
        vault.Add(Sample());
        vault.Save();

        string path = dir.Options.VaultFilePath;
        System.Text.Json.Nodes.JsonNode file = System.Text.Json.Nodes.JsonNode.Parse(File.ReadAllText(path))!;
        byte[] payload = Convert.FromBase64String(file["payload"]!.GetValue<string>());
        payload[0] ^= 0x01;
        file["payload"] = Convert.ToBase64String(payload);
        File.WriteAllText(path, file.ToJsonString());

        Assert.ThrowsExactly<CryptographicException>(vault.Reload);
        Assert.IsFalse(dir.NewVault().Unlock("mypassword"));
    }

    [TestMethod(DisplayName = "Change password keeps entries")]
    public void ChangePassword()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create("mypassword");
        vault.Add(Sample());
        vault.Save();
        vault.ChangePassword("newpassword");

        VaultService reopened = dir.NewVault();
        Assert.IsFalse(reopened.Unlock("mypassword"));
        Assert.IsTrue(reopened.Unlock("newpassword"));
        reopened.Reload();
        Assert.HasCount(1, reopened.Entries);
    }

    [TestMethod(DisplayName = "Lock forgets key and entries")]
    public void Lock()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create("mypassword");
        vault.Add(Sample());
        vault.Lock();

        Assert.IsFalse(vault.IsUnlocked);
        Assert.IsEmpty(vault.Entries);
        Assert.ThrowsExactly<InvalidOperationException>(vault.Save);
        Assert.ThrowsExactly<InvalidOperationException>(vault.Reload);
    }

    [TestMethod(DisplayName = "Load detects a vault replaced by another one")]
    public void ReplacedFile()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create("mypassword");

        using TestVaultDirectory other = new();
        other.NewVault().Create("otherpassword");
        File.Copy(other.Options.VaultFilePath, dir.Options.VaultFilePath, overwrite: true);

        Assert.ThrowsExactly<InvalidOperationException>(vault.Reload);
        Assert.IsFalse(vault.IsUnlocked);
    }

    [TestMethod(DisplayName = "Erase deletes the vault")]
    public void Erase()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create("mypassword");
        vault.Erase();

        Assert.IsFalse(vault.IsInitialized);
        Assert.IsFalse(File.Exists(dir.Options.VaultFilePath));
    }

    [TestMethod(DisplayName = "Download only accepts https")]
    [DataRow("http://example.com/vault")]
    [DataRow("ftp://example.com/vault")]
    [DataRow("not a url")]
    public async Task DownloadRequiresHttps(string url)
    {
        using TestVaultDirectory dir = new();
        await Assert.ThrowsExactlyAsync<ArgumentException>(() => dir.NewVault().DownloadAsync(url));
    }

    [TestMethod(DisplayName = "CSV import: Firefox")]
    public void FirefoxImport()
    {
        const string csv = """
            "url","username","password","httpRealm","formActionOrigin","guid","timeCreated","timeLastUsed","timePasswordChanged"
            "https://site.example","alice","pa,ss","","https://site.example","{1}","1","1","1"
            """;

        MyPassword imported = CsvPasswordImporter.Read(new MemoryStream(Encoding.UTF8.GetBytes(csv))).Single();
        Assert.AreEqual("https://site.example", imported.Data);
        Assert.AreEqual("alice", imported.UserName);
        Assert.AreEqual("pa,ss", imported.Password);
    }
}
