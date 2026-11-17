using VaultCore;

namespace VaultGui.Services;

/// <summary>
/// Locks the vault when the app has been in the background (Android) or minimized (Windows)
/// for longer than the configured delay.
/// </summary>
public sealed class AutoLockService(IVaultService vault, AppSettings settings)
{
    private DateTime? backgroundSince;

    public void Attach(Window window)
    {
        window.Stopped += (_, _) =>
        {
            backgroundSince = DateTime.UtcNow;
            if (settings.AutoLockMinutes <= 0)
                vault.Lock();
        };

        window.Resumed += (_, _) =>
        {
            if (backgroundSince is DateTime since
                && DateTime.UtcNow - since >= TimeSpan.FromMinutes(settings.AutoLockMinutes))
            {
                vault.Lock();
            }
            backgroundSince = null;
        };
    }
}
