using VaultCore;
using VaultCore.Models;

namespace VaultTestProject;

/// <summary>
/// Key export / unlock with key, used by biometric quick unlock.
/// </summary>
[TestClass]
public class UnitTestQuickUnlock
{
    private const string Password = "mypassword";

    [TestMethod(DisplayName = "Exported key unlocks the vault without the password")]
    public void UnlockWithExportedKey()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create(Password);
        vault.Add(new MyPassword { UserName = "alice" });
        vault.Save();
        byte[] key = vault.ExportKey();
        vault.Lock();

        VaultService reopened = dir.NewVault();
        Assert.IsTrue(reopened.UnlockWithKey(key));
        Assert.IsTrue(reopened.IsUnlocked);
        Assert.HasCount(1, reopened.Entries);

        // The caller keeps ownership of its buffer: zeroing it must not break the session.
        Array.Clear(key);
        reopened.Add(new MyPassword { UserName = "bob" });
        reopened.Save();
        Assert.IsTrue(dir.NewVault().Unlock(Password));
    }

    [TestMethod(DisplayName = "Wrong or malformed key is refused")]
    public void WrongKey()
    {
        using TestVaultDirectory dir = new();
        dir.NewVault().Create(Password);

        VaultService vault = dir.NewVault();
        Assert.IsFalse(vault.UnlockWithKey(new byte[32]));
        Assert.IsFalse(vault.UnlockWithKey(new byte[16]));
        Assert.IsFalse(vault.IsUnlocked);
    }

    [TestMethod(DisplayName = "No vault: key unlock fails, key id is null")]
    public void NoVault()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        Assert.IsNull(vault.FileKeyId);
        Assert.IsFalse(vault.UnlockWithKey(new byte[32]));
        Assert.ThrowsExactly<InvalidOperationException>(() => vault.ExportKey());
    }

    [TestMethod(DisplayName = "Password change: KeyChanged raised, key id changes, old key no longer works")]
    public void PasswordChangeInvalidatesKey()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create(Password);
        byte[] oldKey = vault.ExportKey();
        string? oldId = dir.NewVault().FileKeyId;
        Assert.IsNotNull(oldId);

        int raised = 0;
        vault.KeyChanged += (_, _) => raised++;
        vault.ChangePassword("newpassword");

        Assert.AreEqual(1, raised);
        Assert.AreNotEqual(oldId, dir.NewVault().FileKeyId);
        Assert.IsFalse(dir.NewVault().UnlockWithKey(oldKey));
        Assert.IsTrue(dir.NewVault().UnlockWithKey(vault.ExportKey()));
    }

    [TestMethod(DisplayName = "Unlocking with a key keeps the key id stable")]
    public void KeyIdStable()
    {
        using TestVaultDirectory dir = new();
        VaultService vault = dir.NewVault();
        vault.Create(Password);
        string? id = vault.FileKeyId;
        vault.Add(new MyPassword { UserName = "a" });
        vault.Save();
        Assert.AreEqual(id, vault.FileKeyId);
    }
}
