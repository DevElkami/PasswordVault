using System.Security.Cryptography;
using VaultGui.Resources.Strings;
using VaultCore;
using VaultCore.Crypto;

namespace VaultGui.Services;

public enum UpdateOutcome
{
    /// <summary>The local vault was replaced by the downloaded one.</summary>
    Updated,

    /// <summary>The server has the same file as this device.</summary>
    UpToDate,

    /// <summary>Replaced by a vault with another master password: it is now locked.</summary>
    OtherPassword,

    /// <summary>The user kept the local vault.</summary>
    Cancelled,

    /// <summary>The address returns a 3.x file: it cannot replace a vault.</summary>
    LegacyFile,

    /// <summary>The address does not return a vault file.</summary>
    NotAVault,
}

/// <summary>
/// Gets the latest vault from the https address the user entered once (first launch or Settings).
/// The address is remembered, so an update is one tap.
/// </summary>
public sealed class VaultUpdateService(IVaultService vault, AppSettings settings, IDialogService dialogs)
{
    /// <summary>The remembered https address (empty if none).</summary>
    public string Url => settings.DownloadUrl;

    public bool IsConfigured => settings.DownloadUrl.Length > 0;

    public void Remember(string url) => settings.DownloadUrl = url.Trim();

    /// <summary>The local vault file is exactly <paramref name="content"/>, just downloaded.</summary>
    public void MarkDownloaded(byte[] content) => settings.DownloadedHash = Hash(content);

    /// <summary>
    /// Download and replace the local vault. Asks first only when this device has changes of its own
    /// (the local file is no longer the one downloaded last time), since replacing would lose them.
    /// </summary>
    /// <exception cref="ArgumentException">Not an https address.</exception>
    public async Task<UpdateOutcome> UpdateAsync()
    {
        byte[] content = await VaultDownloader.DownloadAsync(settings.DownloadUrl);
        if (VaultService.IsLegacyDataFile(content))
            return UpdateOutcome.LegacyFile;
        if (!VaultCipher.IsVaultFile(content))
            return UpdateOutcome.NotAVault;

        string downloaded = Hash(content);
        string? local = LocalHash();
        if (local == downloaded)
        {
            settings.DownloadedHash = downloaded;
            return UpdateOutcome.UpToDate;
        }

        if (local is not null && local != settings.DownloadedHash && !await dialogs.ConfirmAsync(Loc.Get(Loc.ConfirmReplace)))
            return UpdateOutcome.Cancelled;

        bool samePassword = await Task.Run(() => vault.ImportVaultFile(content));
        settings.DownloadedHash = downloaded;
        // When the vault gets locked, the app shell goes back to the unlock page.
        return samePassword ? UpdateOutcome.Updated : UpdateOutcome.OtherPassword;
    }

    /// <summary>What to tell the user (null: nothing to say).</summary>
    public static string? Message(UpdateOutcome outcome) => outcome switch
    {
        UpdateOutcome.Updated => Loc.Get(Loc.Downloaded),
        UpdateOutcome.UpToDate => Loc.Get(Loc.UpToDate),
        UpdateOutcome.OtherPassword => Loc.Get(Loc.DownloadedOther),
        UpdateOutcome.LegacyFile => Loc.Get(Loc.LegacyInSettings),
        UpdateOutcome.NotAVault => Loc.Get(Loc.NotAVaultFile),
        _ => null,
    };

    private string? LocalHash()
    {
        try
        {
            return vault.IsInitialized && !vault.NeedsMigration ? Hash(vault.ReadVaultFile()) : null;
        }
        catch (IOException)
        {
            return null;
        }
    }

    private static string Hash(byte[] content) => Convert.ToHexString(SHA256.HashData(content));
}
