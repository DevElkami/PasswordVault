using System.Text;
using VaultCore;
using VaultCore.Crypto;
using VaultCore.Import;
using VaultCore.Models;

namespace VaultTestProject;

[TestClass]
public class UnitTestVaultService
{
    private static VaultService NewService(TestVaultDirectory dir) => new(dir.Options);

    [TestMethod(DisplayName = "Create, add, save, unlock again")]
    public async Task RoundTrip()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = NewService(dir);
        await vault.CreateAsync("mypassword");

        Assert.IsTrue(vault.Add(new MyPassword { UserName = "alice", Password = "p", Data = "https://mail.example" }));
        Assert.IsFalse(vault.Add(new MyPassword { UserName = "alice", Password = "p", Data = "https://mail.example" }));
        await vault.SaveAsync();

        VaultService reopened = NewService(dir);
        Assert.IsFalse(await reopened.UnlockAsync("bad"));
        Assert.IsTrue(await reopened.UnlockAsync("mypassword"));
        Assert.HasCount(1, reopened.Entries);
    }

    [TestMethod(DisplayName = "Search is case-insensitive and ignores the password field")]
    public void Search()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = NewService(dir);
        vault.Create("mypassword");
        vault.Add(new MyPassword { UserName = "Alice", Password = "zzz", Data = "https://Mail.example", Keyword = "work" });
        vault.Add(new MyPassword { UserName = "bob", Password = "mail", Data = "https://bank.example" });

        Assert.HasCount(1, vault.Search("MAIL"));
        Assert.HasCount(1, vault.Search("alice"));
        Assert.HasCount(1, vault.Search("Work"));
        Assert.IsEmpty(vault.Search("zzz"));
        Assert.HasCount(2, vault.Search("*"));
        Assert.HasCount(2, vault.Search(""));
    }

    [TestMethod(DisplayName = "Lock raises Locked and clears entries")]
    public void LockedEvent()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = NewService(dir);
        vault.Create("mypassword");
        vault.Add(new MyPassword { UserName = "alice" });

        int raised = 0;
        vault.Locked += (_, _) => raised++;
        vault.Lock();
        vault.Lock();

        Assert.AreEqual(1, raised);
        Assert.IsEmpty(vault.Entries);
        Assert.ThrowsExactly<InvalidOperationException>(() => vault.Add(new MyPassword()));
    }

    [TestMethod(DisplayName = "Importing a newer copy of the same vault keeps it unlocked")]
    public void ImportSameVault()
    {
        using TestVaultDirectory pc = new();
        VaultService pcVault = NewService(pc);
        pcVault.Create("mypassword");
        pcVault.Save();

        // Phone gets a first copy and unlocks it.
        using TestVaultDirectory phone = new();
        VaultService phoneVault = NewService(phone);
        Assert.IsFalse(phoneVault.ImportVaultFile(File.ReadAllBytes(pc.Options.VaultFilePath)));
        Assert.IsTrue(phoneVault.Unlock("mypassword"));

        // PC adds an entry; the phone downloads again without asking the password.
        pcVault.Add(new MyPassword { UserName = "new" });
        pcVault.Save();
        Assert.IsTrue(phoneVault.ImportVaultFile(File.ReadAllBytes(pc.Options.VaultFilePath)));
        Assert.AreEqual("new", phoneVault.Entries.Single().UserName);
    }

    [TestMethod(DisplayName = "Importing another vault locks and asks for its password")]
    public void ImportOtherVault()
    {
        using TestVaultDirectory other = new();
        VaultService otherVault = NewService(other);
        otherVault.Create("otherpassword");

        using TestVaultDirectory dir = new();
        VaultService vault = NewService(dir);
        vault.Create("mypassword");

        Assert.IsFalse(vault.ImportVaultFile(File.ReadAllBytes(other.Options.VaultFilePath)));
        Assert.IsFalse(vault.IsUnlocked);
        Assert.IsTrue(vault.Unlock("otherpassword"));
    }

    [TestMethod(DisplayName = "Importing a v1 file is refused")]
    public void ImportV1Refused()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = NewService(dir);
        vault.Create("mypassword");

        Assert.ThrowsExactly<VaultFormatException>(() => vault.ImportVaultFile(Encoding.UTF8.GetBytes("bGluZTE=\nbGluZTI=")));
        Assert.IsTrue(vault.IsUnlocked);
    }

    [TestMethod(DisplayName = "CSV import: Chrome / Edge")]
    public void ImportChrome()
    {
        const string csv = "name,url,username,password,note\nmail.example,https://mail.example/,alice,p1,\n";
        MyPassword entry = CsvPasswordImporter.Read(new MemoryStream(Encoding.UTF8.GetBytes(csv))).Single();
        Assert.AreEqual("https://mail.example/", entry.Data);
        Assert.AreEqual("alice", entry.UserName);
        Assert.AreEqual("p1", entry.Password);
        Assert.AreEqual("mail.example", entry.Keyword);
    }

    [TestMethod(DisplayName = "CSV import: Bitwarden")]
    public void ImportBitwarden()
    {
        const string csv = "folder,favorite,type,name,notes,fields,reprompt,login_uri,login_username,login_password,login_totp\n"
            + ",,login,Bank,,,0,https://bank.example,bob,\"p,2\",\n"
            + ",,note,Secure note,text,,0,,,,\n";
        MyPassword entry = CsvPasswordImporter.Read(new MemoryStream(Encoding.UTF8.GetBytes(csv))).Single();
        Assert.AreEqual("https://bank.example", entry.Data);
        Assert.AreEqual("bob", entry.UserName);
        Assert.AreEqual("p,2", entry.Password);
        Assert.AreEqual("Bank", entry.Keyword);
    }

    [TestMethod(DisplayName = "CSV import: unknown format is rejected")]
    public void ImportUnknown()
    {
        Assert.ThrowsExactly<InvalidDataException>(() =>
            CsvPasswordImporter.Read(new MemoryStream(Encoding.UTF8.GetBytes("a,b\n1,2\n"))));
        Assert.IsEmpty(CsvPasswordImporter.Read(new MemoryStream()));
    }

    [TestMethod(DisplayName = "List row: site first, keyword only as a title when there is no site")]
    public void DisplayLines()
    {
        MyPassword full = new() { Data = "https://mail.example", UserName = "alice", Keyword = "work mail" };
        Assert.AreEqual("https://mail.example", full.Title);
        Assert.AreEqual("work mail", full.KeywordLine);

        MyPassword noSite = new() { UserName = "alice", Keyword = "wifi" };
        Assert.AreEqual("wifi", noSite.Title);
        Assert.AreEqual("", noSite.KeywordLine);

        MyPassword noKeyword = new() { Data = "https://mail.example" };
        Assert.AreEqual("https://mail.example", noKeyword.Title);
        Assert.AreEqual("", noKeyword.KeywordLine);

        List<string?> changed = [];
        full.PropertyChanged += (_, e) => changed.Add(e.PropertyName);
        full.Data = "";
        CollectionAssert.Contains(changed, nameof(MyPassword.Title));
        CollectionAssert.Contains(changed, nameof(MyPassword.KeywordLine));
        Assert.DoesNotContain("KeywordLine", full.ToStr());
    }
}
