using VaultCore;
using VaultCore.Crypto;
using VaultCore.Models;

namespace VaultTestProject;

[TestClass]
public class UnitTestMigration
{
    private static void WriteV1(TestVaultDirectory dir, string password, params MyPassword[] entries) =>
        OriginalV1Writer.WriteVault(dir.Options.LegacyKeyFilePath!, dir.Options.LegacyDataFilePath!, password, entries);

    [TestMethod(DisplayName = "v1 vault is migrated to v2 on first unlock")]
    public void MigratesOnUnlock()
    {
        using TestVaultDirectory dir = new();
        WriteV1(dir, "oldpassword",
            new MyPassword { UserName = "alice", Password = "p1", Data = "https://a.example", Keyword = "mail" },
            new MyPassword { UserName = "bob", Password = "p2", Data = "https://b.example" });

        VaultService vault = dir.NewVault();
        Assert.IsTrue(vault.IsInitialized);
        Assert.IsTrue(vault.NeedsMigration);

        Assert.IsTrue(vault.Unlock("oldpassword"));
        vault.Reload();

        Assert.HasCount(2, vault.Entries);
        Assert.AreEqual("p1", vault.Entries.Single(x => x.UserName == "alice").Password);
        Assert.AreEqual("mail", vault.Entries.Single(x => x.UserName == "alice").Keyword);

        // v2 written, old files deleted without backup.
        Assert.IsFalse(vault.NeedsMigration);
        Assert.IsTrue(VaultCipher.IsVaultFile(File.ReadAllBytes(dir.Options.VaultFilePath)));
        Assert.IsFalse(File.Exists(dir.Options.LegacyKeyFilePath));
        Assert.IsFalse(File.Exists(dir.Options.LegacyDataFilePath));
        Assert.IsFalse(File.Exists(dir.Options.LegacyKeyFilePath + ".v1.bak"));
        Assert.IsFalse(File.Exists(dir.Options.LegacyDataFilePath + ".v1.bak"));

        // Same password still works afterwards, now through v2.
        VaultService reopened = dir.NewVault();
        Assert.IsTrue(reopened.Unlock("oldpassword"));
        reopened.Reload();
        Assert.HasCount(2, reopened.Entries);
    }

    [TestMethod(DisplayName = "Wrong password leaves the v1 vault untouched")]
    public void WrongPasswordNoMigration()
    {
        using TestVaultDirectory dir = new();
        WriteV1(dir, "oldpassword", new MyPassword { UserName = "alice" });

        VaultService vault = dir.NewVault();
        Assert.IsFalse(vault.Unlock("badpassword"));

        Assert.IsTrue(vault.NeedsMigration);
        Assert.IsFalse(File.Exists(dir.Options.VaultFilePath));
        Assert.IsTrue(File.Exists(dir.Options.LegacyKeyFilePath));
        Assert.IsTrue(File.Exists(dir.Options.LegacyDataFilePath));
    }

    [TestMethod(DisplayName = "v1 key without data file migrates to an empty vault")]
    public void KeyOnly()
    {
        using TestVaultDirectory dir = new();
        WriteV1(dir, "oldpassword");
        File.Delete(dir.Options.LegacyDataFilePath!);

        VaultService vault = dir.NewVault();
        Assert.IsTrue(vault.Unlock("oldpassword"));
        vault.Reload();
        Assert.IsEmpty(vault.Entries);
        Assert.IsTrue(File.Exists(dir.Options.VaultFilePath));
    }

    [TestMethod(DisplayName = "When both formats exist, v2 wins")]
    public void V2TakesPrecedence()
    {
        using TestVaultDirectory dir = new();
        dir.NewVault().Create("newpassword");
        WriteV1(dir, "oldpassword", new MyPassword { UserName = "alice" });

        VaultService vault = dir.NewVault();
        Assert.IsFalse(vault.NeedsMigration);
        Assert.IsFalse(vault.Unlock("oldpassword"));
        Assert.IsTrue(vault.Unlock("newpassword"));
    }

    private static byte[] DownloadedV1File(string password, params MyPassword[] entries)
    {
        using TestVaultDirectory server = new();
        WriteV1(server, password, entries);
        return File.ReadAllBytes(server.Options.LegacyDataFilePath!);
    }

    [TestMethod(DisplayName = "3.x data file downloaded from a server: converted with its password")]
    public void CreateFromDownloadedV1()
    {
        byte[] content = DownloadedV1File("oldpassword",
            new MyPassword { UserName = "alice", Password = "p1", Data = "https://a.example" },
            new MyPassword { UserName = "bob", Password = "p2" });
        Assert.IsTrue(VaultService.IsLegacyDataFile(content));

        using TestVaultDirectory phone = new();
        VaultService vault = phone.NewVault();
        Assert.IsTrue(vault.CreateFromLegacyData(content, "oldpassword"));
        Assert.IsTrue(vault.IsUnlocked);
        Assert.HasCount(2, vault.Entries);

        // Saved as v2, same password.
        VaultService reopened = phone.NewVault();
        Assert.IsTrue(reopened.Unlock("oldpassword"));
        Assert.AreEqual("p1", reopened.Entries.Single(x => x.UserName == "alice").Password);
    }

    [TestMethod(DisplayName = "3.x data file with the wrong password: refused, nothing created")]
    public void DownloadedV1WrongPassword()
    {
        byte[] content = DownloadedV1File("oldpassword", new MyPassword { UserName = "alice" }, new MyPassword { UserName = "bob" });

        using TestVaultDirectory phone = new();
        VaultService vault = phone.NewVault();
        Assert.IsFalse(vault.CreateFromLegacyData(content, "badpassword"));
        Assert.IsFalse(vault.IsInitialized);
        Assert.IsFalse(vault.IsUnlocked);
    }

    [TestMethod(DisplayName = "Only real 3.x data files are accepted, never over an existing vault")]
    public void DownloadedV1Checks()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        byte[] notAVault = System.Text.Encoding.UTF8.GetBytes("<html>404 Not Found</html>");
        Assert.IsFalse(VaultService.IsLegacyDataFile(notAVault));
        Assert.IsFalse(VaultService.IsLegacyDataFile([]));
        Assert.ThrowsExactly<VaultFormatException>(() => vault.CreateFromLegacyData(notAVault, "x"));

        vault.Create("newpassword");
        Assert.IsFalse(VaultService.IsLegacyDataFile(File.ReadAllBytes(dir.Options.VaultFilePath)));
        byte[] content = DownloadedV1File("oldpassword", new MyPassword { UserName = "alice" });
        Assert.ThrowsExactly<InvalidOperationException>(() => vault.CreateFromLegacyData(content, "oldpassword"));
    }

    [TestMethod(DisplayName = "3.x data file imported into an existing vault: entries added once")]
    public void ImportV1IntoExistingVault()
    {
        byte[] content = DownloadedV1File("oldpassword",
            new MyPassword { UserName = "alice", Password = "p1", Data = "https://a.example" },
            new MyPassword { UserName = "bob", Password = "p2" });

        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create("newpassword");
        Assert.IsTrue(vault.Add(new MyPassword { UserName = "bob", Password = "p2" }));
        Assert.IsTrue(vault.Add(new MyPassword { UserName = "carol", Password = "p3" }));

        // "bob" is already there.
        Assert.AreEqual(1, vault.ImportLegacyData(content, "oldpassword"));
        Assert.HasCount(3, vault.Entries);
        Assert.AreEqual(0, vault.ImportLegacyData(content, "oldpassword"));

        // Saved, and the vault keeps its own password.
        VaultService reopened = dir.NewVault();
        Assert.IsTrue(reopened.Unlock("newpassword"));
        Assert.AreEqual("p1", reopened.Entries.Single(x => x.UserName == "alice").Password);
        Assert.HasCount(3, reopened.Entries);
    }

    [TestMethod(DisplayName = "3.x import: wrong password or wrong file changes nothing")]
    public void ImportV1Refused()
    {
        byte[] content = DownloadedV1File("oldpassword", new MyPassword { UserName = "alice" }, new MyPassword { UserName = "bob" });

        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        Assert.ThrowsExactly<InvalidOperationException>(() => vault.ImportLegacyData(content, "oldpassword")); // Locked.

        vault.Create("newpassword");
        Assert.IsNull(vault.ImportLegacyData(content, "badpassword"));
        Assert.ThrowsExactly<VaultFormatException>(() => vault.ImportLegacyData(System.Text.Encoding.UTF8.GetBytes("<html>"), "x"));
        Assert.ThrowsExactly<VaultFormatException>(() => vault.ImportLegacyData(File.ReadAllBytes(dir.Options.VaultFilePath), "newpassword"));
        Assert.HasCount(0, vault.Entries);
    }
}
