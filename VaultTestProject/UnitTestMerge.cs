using VaultCore;
using VaultCore.Crypto;
using VaultCore.Models;

namespace VaultTestProject;

/// <summary>
/// Two instances of the app working on the same vault file: a save merges what the other one wrote
/// instead of overwriting it.
/// </summary>
[TestClass]
public class UnitTestMerge
{
    private const string Password = "mypassword";

    private sealed class ManualClock : TimeProvider
    {
        public DateTimeOffset Now { get; set; } = new(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
        public override DateTimeOffset GetUtcNow() => Now;
        public void Advance(int minutes = 1) => Now = Now.AddMinutes(minutes);
    }

    /// <summary>One vault file with <paramref name="users"/>, opened by two instances A and B.</summary>
    private sealed class Setup : IDisposable
    {
        public TestVaultDirectory Dir { get; } = new();
        public ManualClock Clock { get; } = new();
        public VaultService A { get; }
        public VaultService B { get; }

        public Setup(params string[] users)
        {
            A = Open();
            A.Create(Password);
            foreach (string user in users)
                Assert.IsTrue(A.Add(new MyPassword { UserName = user, Password = "p-" + user, Data = "https://" + user }));
            A.Save();

            B = Open();
            Assert.IsTrue(B.Unlock(Password));
            Clock.Advance();
        }

        public VaultService Open() => new(new VaultOptions
        {
            VaultFilePath = Dir.Options.VaultFilePath,
            Pbkdf2Iterations = TestVaultDirectory.FastIterations,
            TimeProvider = Clock,
        });

        public VaultService Reopened()
        {
            VaultService vault = Open();
            Assert.IsTrue(vault.Unlock(Password));
            return vault;
        }

        public void Dispose() => Dir.Dispose();
    }

    private static MyPassword Entry(VaultService vault, string user) => vault.Entries.Single(x => x.UserName == user);

    private static string[] Users(VaultService vault) => [.. vault.Entries.Select(x => x.UserName)];

    [TestMethod(DisplayName = "Entries added by both instances are all kept")]
    public void AddOnBoth()
    {
        using Setup s = new("common");
        s.A.Add(new MyPassword { UserName = "a-only" });
        s.A.Save();
        s.B.Add(new MyPassword { UserName = "b-only" });
        s.B.Save();
        s.A.Reload();

        CollectionAssert.AreEquivalent(new[] { "common", "a-only", "b-only" }, Users(s.A));
        CollectionAssert.AreEquivalent(new[] { "common", "a-only", "b-only" }, Users(s.B));
        CollectionAssert.AreEquivalent(new[] { "common", "a-only", "b-only" }, Users(s.Reopened()));
    }

    [TestMethod(DisplayName = "A deletion is not undone by an instance that still has the entry")]
    public void DeleteIsKept()
    {
        using Setup s = new("a", "b");
        s.A.Remove(Entry(s.A, "a"));
        s.A.Save();

        // B still has "a" in memory: saving must not bring it back.
        s.B.Add(new MyPassword { UserName = "c" });
        s.B.Save();

        CollectionAssert.AreEquivalent(new[] { "b", "c" }, Users(s.B));
        CollectionAssert.AreEquivalent(new[] { "b", "c" }, Users(s.Reopened()));
    }

    [TestMethod(DisplayName = "Same entry edited by both instances: the newest edit wins, whoever saves last")]
    public void NewestEditWins()
    {
        using Setup s = new("a");
        MyPassword inA = Entry(s.A, "a");
        Assert.IsTrue(s.A.Update(inA, new MyPassword { UserName = "a", Password = "older", Data = inA.Data }));

        s.Clock.Advance();
        MyPassword inB = Entry(s.B, "a");
        Assert.IsTrue(s.B.Update(inB, new MyPassword { UserName = "a", Password = "newer", Data = inB.Data }));
        s.B.Save();

        // A saves after B, with an older edit.
        s.A.Save();

        Assert.AreEqual("newer", Entry(s.A, "a").Password);
        VaultService reopened = s.Reopened();
        Assert.HasCount(1, reopened.Entries);
        Assert.AreEqual("newer", Entry(reopened, "a").Password);
    }

    [TestMethod(DisplayName = "An edit made after a deletion keeps the entry")]
    public void EditAfterDeleteWins()
    {
        using Setup s = new("a");
        s.A.Remove(Entry(s.A, "a"));
        s.A.Save();

        s.Clock.Advance();
        s.B.Update(Entry(s.B, "a"), new MyPassword { UserName = "a", Password = "kept" });
        s.B.Save();
        s.A.Reload();

        Assert.AreEqual("kept", Entry(s.A, "a").Password);
        Assert.AreEqual("kept", Entry(s.Reopened(), "a").Password);
    }

    [TestMethod(DisplayName = "The same entry added by both instances is kept once")]
    public void SameAddOnBoth()
    {
        using Setup s = new();
        s.A.Add(new MyPassword { UserName = "twin", Password = "x" });
        s.A.Save();
        s.Clock.Advance();
        s.B.Add(new MyPassword { UserName = "twin", Password = "x" });
        s.B.Save();
        s.A.Reload();

        Assert.HasCount(1, s.A.Entries);
        Assert.HasCount(1, s.B.Entries);
        Assert.AreEqual(Entry(s.A, "twin").Id, Entry(s.B, "twin").Id);
    }

    [TestMethod(DisplayName = "Entries saved before ids existed always get the same id")]
    public void LegacyEntriesGetStableIds()
    {
        using TestVaultDirectory dir = new();
        dir.NewVault().Create(Password);

        // Rewrite the file as an early 4.0 build did: entries without id or date.
        VaultKdfParameters kdf = VaultCipher.Parse(File.ReadAllBytes(dir.Options.VaultFilePath)).Kdf;
        byte[] key = VaultCipher.DeriveKey(Password, kdf);
        byte[] payload = System.Text.Encoding.UTF8.GetBytes("""{"entries":[{"Data":"d","UserName":"u","Password":"p","Keyword":"k"}]}""");
        File.WriteAllBytes(dir.Options.VaultFilePath, VaultCipher.Seal(payload, key, kdf));

        VaultService first = dir.NewVault();
        VaultService second = dir.NewVault();
        Assert.IsTrue(first.Unlock(Password));
        Assert.IsTrue(second.Unlock(Password));

        string id = first.Entries.Single().Id;
        Assert.IsFalse(string.IsNullOrEmpty(id));
        Assert.AreEqual(id, second.Entries.Single().Id);
    }

    [TestMethod(DisplayName = "Vault file rewritten without truncation (stale bytes at the end) still opens")]
    public void TrailingBytesTolerated()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create(Password);
        vault.Add(new MyPassword { UserName = "a" });
        vault.Save();

        byte[] content = File.ReadAllBytes(dir.Options.VaultFilePath);
        File.WriteAllBytes(dir.Options.VaultFilePath, [.. content, .. "\"}stale-end-of-a-longer-file}"u8]);

        VaultService reopened = dir.NewVault();
        Assert.IsTrue(reopened.Unlock(Password));
        Assert.HasCount(1, reopened.Entries);
    }

    [TestMethod(DisplayName = "Update keeps the id and refuses duplicates")]
    public void UpdateKeepsId()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create(Password);
        vault.Add(new MyPassword { UserName = "a" });
        vault.Add(new MyPassword { UserName = "b" });
        MyPassword a = vault.Entries.Single(x => x.UserName == "a");

        Assert.IsFalse(vault.Update(a, new MyPassword { UserName = "b" }));
        MyPassword edited = new() { UserName = "a2" };
        Assert.IsTrue(vault.Update(a, edited));
        Assert.AreEqual(a.Id, edited.Id);
        Assert.IsGreaterThan(a.Modified, edited.Modified);
        Assert.HasCount(2, vault.Entries);
    }

    [TestMethod(DisplayName = "Password changed by another instance: this one is locked, nothing overwritten")]
    public void PasswordChangedElsewhere()
    {
        using Setup s = new("a");
        s.A.ChangePassword("newpassword");

        s.B.Add(new MyPassword { UserName = "b-only" });
        Assert.ThrowsExactly<InvalidOperationException>(s.B.Save);
        Assert.IsFalse(s.B.IsUnlocked);

        VaultService reopened = s.Open();
        Assert.IsFalse(reopened.Unlock(Password));
        Assert.IsTrue(reopened.Unlock("newpassword"));
        CollectionAssert.AreEquivalent(new[] { "a" }, Users(reopened));
    }
}
