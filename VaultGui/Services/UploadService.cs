using System.Globalization;
using System.Security.Cryptography;
using VaultGui.Resources.Strings;
using VaultCore;
using VaultCore.Upload;

namespace VaultGui.Services;

/// <summary>
/// Sends the encrypted vault file to the user's server, or deletes it there, on request only (never automatically).
/// <para>
/// The server password is kept in the platform secure storage (encrypted for the current Windows account),
/// never in the plain preferences. The server's identity (SSH host key, or a TLS certificate Windows does not
/// trust) is confirmed by the user the first time and must stay the same afterwards.
/// </para>
/// </summary>
public sealed class UploadService
{
    private const string PasswordKey = "UPLOAD_PASSWORD";

    private readonly IVaultService vault;
    private readonly AppSettings settings;
    private readonly IVaultUploader uploader;
    private readonly IDialogService dialogs;

    public UploadService(IVaultService vault, AppSettings settings, IVaultUploader uploader, IDialogService dialogs)
    {
        this.vault = vault;
        this.settings = settings;
        this.uploader = uploader;
        this.dialogs = dialogs;

        vault.Saved += (_, _) => StateChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Raised (on any thread) when <see cref="HasUnsentChanges"/> may have changed.</summary>
    public event EventHandler? StateChanged;

    public bool IsSupported => uploader.IsSupported;

    public bool IsConfigured => IsSupported && settings.UploadAddress.Length > 0 && settings.UploadUser.Length > 0;

    public string Address => settings.UploadAddress;

    public string User => settings.UploadUser;

    public async Task<bool> HasStoredPasswordAsync() => !string.IsNullOrEmpty(await SecureStorage.Default.GetAsync(PasswordKey));

    /// <summary>The local vault file is not the one last sent to the server.</summary>
    public bool HasUnsentChanges
    {
        get
        {
            // Deleted from the server on purpose: no reminder until it is sent again.
            if (!IsConfigured || settings.UploadDeletedAt.Length > 0)
                return false;
            byte[]? content = ReadLocalFile();
            return content is not null && Hash(content) != settings.UploadedHash;
        }
    }

    public string StatusText
    {
        get
        {
            if (!IsConfigured)
                return "";
            if (DateTime.TryParse(settings.UploadDeletedAt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime deletedAt))
                return Loc.Format(Loc.UploadDeletedAt, deletedAt.ToLocalTime());
            if (HasUnsentChanges)
                return Loc.Get(Loc.UploadPending);
            return DateTime.TryParse(settings.UploadedAt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime at)
                ? Loc.Format(Loc.UploadDoneAt, at.ToLocalTime())
                : "";
        }
    }

    /// <summary>Remember where to send the vault.</summary>
    /// <param name="newPassword">Null or empty keeps the stored password.</param>
    /// <exception cref="FormatException">Bad address.</exception>
    public async Task ConfigureAsync(string address, string user, string? newPassword)
    {
        string normalized = address.Trim();
        UploadAddress.Parse(normalized);

        if (normalized != settings.UploadAddress)
        {
            // Another file or server: nothing was sent there yet.
            settings.UploadedHash = "";
            settings.UploadedAt = "";
            settings.UploadDeletedAt = "";
        }

        settings.UploadAddress = normalized;
        settings.UploadUser = user.Trim();
        if (!string.IsNullOrEmpty(newPassword))
            await SecureStorage.Default.SetAsync(PasswordKey, newPassword);
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Forget the server: address, user, password, identity. Nothing is deleted on the server.</summary>
    public void Forget()
    {
        settings.UploadAddress = "";
        settings.UploadUser = "";
        settings.UploadServerPin = "";
        settings.UploadedHash = "";
        settings.UploadedAt = "";
        settings.UploadDeletedAt = "";
        SecureStorage.Default.Remove(PasswordKey);
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Send the vault file and tell the user how it went. Unexpected errors (network, permissions) are thrown.
    /// </summary>
    public async Task<bool> SendAsync()
    {
        if (!IsConfigured || await SecureStorage.Default.GetAsync(PasswordKey) is not { Length: > 0 } password)
        {
            await dialogs.AlertAsync(Loc.Get(Loc.UploadMissing));
            return false;
        }

        UploadAddress address = UploadAddress.Parse(settings.UploadAddress);
        byte[] content = vault.ReadVaultFile();

        try
        {
            if (!await RunTrustedAsync(address, trust => uploader.UploadAsync(address, settings.UploadUser, password, content, trust)))
                return false;
        }
        catch (UploadFailedException exception)
        {
            await dialogs.AlertAsync(Loc.Get(exception.Reason == UploadFailure.Authentication ? Loc.UploadAuthFailed : Loc.UploadNoTls), Loc.Get(Loc.Error));
            return false;
        }

        settings.UploadedHash = Hash(content);
        settings.UploadedAt = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
        settings.UploadDeletedAt = "";
        StateChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Delete the published file from the server (the server settings are kept, to send it again later).
    /// Unexpected errors (network, permissions) are thrown.
    /// </summary>
    public async Task<bool> DeleteRemoteAsync()
    {
        if (!IsConfigured || await SecureStorage.Default.GetAsync(PasswordKey) is not { Length: > 0 } password)
        {
            await dialogs.AlertAsync(Loc.Get(Loc.UploadMissing));
            return false;
        }

        UploadAddress address = UploadAddress.Parse(settings.UploadAddress);

        try
        {
            if (!await RunTrustedAsync(address, trust => uploader.DeleteAsync(address, settings.UploadUser, password, trust)))
                return false;
        }
        catch (UploadFailedException exception)
        {
            await dialogs.AlertAsync(Loc.Get(exception.Reason == UploadFailure.Authentication ? Loc.UploadAuthFailed : Loc.UploadNoTls), Loc.Get(Loc.Error));
            return false;
        }

        settings.UploadedHash = "";
        settings.UploadedAt = "";
        settings.UploadDeletedAt = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
        StateChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Connect; if the server's identity is not the remembered one, drop the connection, ask the user,
    /// then connect again. (Asking during the handshake would let it time out while the user reads.)
    /// </summary>
    /// <param name="transfer">What to do on the server, given the identity check.</param>
    private async Task<bool> RunTrustedAsync(UploadAddress address, Func<Func<ServerIdentity, bool>, Task> transfer)
    {
        ServerIdentity? presented = null;
        bool Check(ServerIdentity identity)
        {
            presented = identity;
            return ServerPin.Evaluate(settings.UploadServerPin, address, identity) == ServerTrust.Known;
        }

        try
        {
            await transfer(Check);
            return true;
        }
        catch (ServerNotTrustedException) when (presented is not null)
        {
            ServerIdentity identity = presented;
            bool firstTime = ServerPin.Evaluate(settings.UploadServerPin, address, identity) == ServerTrust.FirstTime;
            string message = Loc.Format(firstTime ? Loc.UploadTrustFirst : Loc.UploadTrustChanged, address.Host, identity.Description, identity.Fingerprint);
            if (!await dialogs.ConfirmAsync(message, Loc.Get(Loc.UploadTrustTitle)))
                return false;

            settings.UploadServerPin = ServerPin.Create(address, identity);
        }

        // Second connection: only the identity just accepted passes.
        await transfer(Check);
        return true;
    }

    private byte[]? ReadLocalFile()
    {
        try
        {
            return vault.IsInitialized && !vault.NeedsMigration ? vault.ReadVaultFile() : null;
        }
        catch (IOException)
        {
            return null;
        }
    }

    private static string Hash(byte[] content) => Convert.ToHexString(SHA256.HashData(content));
}
