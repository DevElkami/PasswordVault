namespace VaultGui.Services;

/// <summary>
/// User preferences (not secret: stored in plain platform preferences).
/// </summary>
public sealed class AppSettings
{
    // Same key as version 3.x, so the saved URL is kept.
    private const string DownloadUrlKey = "UPDATE_URL";
    private const string AutoLockMinutesKey = "AUTO_LOCK_MINUTES";
    private const string ThemeKey = "APP_THEME";
    private const string QuickUnlockKeyKey = "QUICK_UNLOCK_KEY";
    private const string QuickUnlockKeyIdKey = "QUICK_UNLOCK_KEY_ID";
    private const string QuickUnlockTurnedOffKey = "QUICK_UNLOCK_TURNED_OFF";

    public string DownloadUrl
    {
        get => Preferences.Default.Get(DownloadUrlKey, "");
        set => Preferences.Default.Set(DownloadUrlKey, value);
    }

    /// <summary>Light, dark, or (Unspecified) the theme of the system.</summary>
    public AppTheme Theme
    {
        get => (AppTheme)Preferences.Default.Get(ThemeKey, (int)AppTheme.Unspecified);
        set => Preferences.Default.Set(ThemeKey, (int)value);
    }

    /// <summary>0 = lock as soon as the app goes to the background.</summary>
    public int AutoLockMinutes
    {
        get => Preferences.Default.Get(AutoLockMinutesKey, 1);
        set => Preferences.Default.Set(AutoLockMinutesKey, value);
    }

    /// <summary>SHA-256 of the vault file as last downloaded, to tell whether this device changed it since.</summary>
    public string DownloadedHash
    {
        get => Preferences.Default.Get("DOWNLOADED_HASH", "");
        set => Preferences.Default.Set("DOWNLOADED_HASH", value);
    }

    /// <summary>Vault key protected by the platform (unusable without the biometric check); empty = quick unlock off.</summary>
    public string QuickUnlockKey
    {
        get => Preferences.Default.Get(QuickUnlockKeyKey, "");
        set => Preferences.Default.Set(QuickUnlockKeyKey, value);
    }

    /// <summary><see cref="VaultCore.IVaultService.FileKeyId"/> the protected key belongs to.</summary>
    public string QuickUnlockKeyId
    {
        get => Preferences.Default.Get(QuickUnlockKeyIdKey, "");
        set => Preferences.Default.Set(QuickUnlockKeyIdKey, value);
    }

    /// <summary>Quick unlock was turned off automatically (password or biometrics changed): tell the user.</summary>
    public bool QuickUnlockTurnedOff
    {
        get => Preferences.Default.Get(QuickUnlockTurnedOffKey, false);
        set => Preferences.Default.Set(QuickUnlockTurnedOffKey, value);
    }

    /// <summary>The user was already asked (once) whether to turn quick unlock on.</summary>
    public bool QuickUnlockOffered
    {
        get => Preferences.Default.Get("QUICK_UNLOCK_OFFERED", false);
        set => Preferences.Default.Set("QUICK_UNLOCK_OFFERED", value);
    }

    #region Send to server (the password is not here: see UploadService)
    /// <summary>sftp:// or ftps:// address of the published vault file; empty = not set up.</summary>
    public string UploadAddress
    {
        get => Preferences.Default.Get("UPLOAD_ADDRESS", "");
        set => Preferences.Default.Set("UPLOAD_ADDRESS", value);
    }

    public string UploadUser
    {
        get => Preferences.Default.Get("UPLOAD_USER", "");
        set => Preferences.Default.Set("UPLOAD_USER", value);
    }

    /// <summary>Server identity accepted by the user (see VaultCore.Upload.ServerPin). Not secret.</summary>
    public string UploadServerPin
    {
        get => Preferences.Default.Get("UPLOAD_SERVER_PIN", "");
        set => Preferences.Default.Set("UPLOAD_SERVER_PIN", value);
    }

    /// <summary>SHA-256 of the vault file last sent, to tell whether there are unsent changes.</summary>
    public string UploadedHash
    {
        get => Preferences.Default.Get("UPLOADED_HASH", "");
        set => Preferences.Default.Set("UPLOADED_HASH", value);
    }

    /// <summary>When the vault was last sent (UTC, round-trip format).</summary>
    public string UploadedAt
    {
        get => Preferences.Default.Get("UPLOADED_AT", "");
        set => Preferences.Default.Set("UPLOADED_AT", value);
    }

    /// <summary>When the file was deleted from the server on purpose (round-trip date); empty once sent again.</summary>
    public string UploadDeletedAt
    {
        get => Preferences.Default.Get("UPLOAD_DELETED_AT", "");
        set => Preferences.Default.Set("UPLOAD_DELETED_AT", value);
    }
    #endregion
}
