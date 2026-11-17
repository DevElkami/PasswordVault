using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using VaultCore;

namespace VaultGui.Services;

/// <summary>
/// Unlock with biometrics (Android) or Windows Hello instead of typing the master password.
/// <para>
/// The derived vault key (never the password) is stored protected by <see cref="IBiometricKeyProtector"/>.
/// It is tied to the current vault key: after a master password change
/// quick unlock turns itself off until it is enabled again.
/// </para>
/// </summary>
public sealed class QuickUnlockService
{
    private readonly IVaultService vault;
    private readonly AppSettings settings;
    private readonly IBiometricKeyProtector protector;
    private readonly ILogger<QuickUnlockService> logger;

    public QuickUnlockService(IVaultService vault, AppSettings settings, IBiometricKeyProtector protector, ILogger<QuickUnlockService> logger)
    {
        this.vault = vault;
        this.settings = settings;
        this.protector = protector;
        this.logger = logger;

        if (protector is NoBiometricKeyProtector)
            ForgetLeftovers();

        vault.KeyChanged += (_, _) =>
        {
            if (IsEnabled)
            {
                Disable();
                settings.QuickUnlockTurnedOff = true;
            }
        };
    }

    /// <summary>
    /// No quick unlock on this platform: drop anything stored, so the unlock page only asks for the master password.
    /// </summary>
    private void ForgetLeftovers()
    {
        settings.QuickUnlockTurnedOff = false;
        settings.QuickUnlockKey = "";
        settings.QuickUnlockKeyId = "";
    }

    public string Name => protector.Name;

    public Task<bool> IsAvailableAsync() => protector.IsAvailableAsync();

    public bool IsEnabled => settings.QuickUnlockKey.Length > 0;

    /// <summary>Enabled and still matching the vault file (no password change since).</summary>
    public bool IsUsable => IsEnabled && settings.QuickUnlockKeyId == vault.FileKeyId;

    /// <summary>Turn on (vault must be unlocked). False if the user cancelled the check.</summary>
    public async Task<bool> EnableAsync()
    {
        byte[] key = vault.ExportKey();
        try
        {
            string? protectedKey = await protector.ProtectAsync(key);
            if (protectedKey is null)
                return false;

            settings.QuickUnlockKey = protectedKey;
            settings.QuickUnlockKeyId = vault.FileKeyId ?? "";
            settings.QuickUnlockTurnedOff = false;
            return true;
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
        }
    }

    public void Disable()
    {
        settings.QuickUnlockKey = "";
        settings.QuickUnlockKeyId = "";
        protector.Forget();
    }

    /// <summary>
    /// Ask for the biometric check and unlock. False if cancelled; quick unlock is turned off
    /// (and false returned) when its key no longer opens the vault.
    /// </summary>
    public async Task<bool> UnlockAsync()
    {
        if (!IsUsable)
            return false;

        byte[]? key;
        try
        {
            key = await protector.UnprotectAsync(settings.QuickUnlockKey);
        }
        catch (BiometricInvalidatedException exception)
        {
            logger.LogWarning(exception, "Quick unlock key invalidated");
            Disable();
            settings.QuickUnlockTurnedOff = true;
            return false;
        }

        if (key is null)
            return false;

        try
        {
            if (await vault.UnlockWithKeyAsync(key))
                return true;
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
        }

        Disable();
        settings.QuickUnlockTurnedOff = true;
        return false;
    }
}
