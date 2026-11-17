using VaultGui.Views;
using VaultCore;

namespace VaultGui;

public partial class AppShell : Shell
{
    public AppShell(IVaultService vault)
    {
        InitializeComponent();

        Routing.RegisterRoute("entry", typeof(EntryPage));
        Routing.RegisterRoute("settings", typeof(SettingsPage));

        CurrentItem = vault.IsInitialized ? UnlockItem : SetupItem;

#if ANDROID
        if (Application.Current is { } app)
            app.RequestedThemeChanged += (_, _) => ApplySystemBars();
#endif

        // Manual lock, auto-lock, or a vault replaced by one with another password.
        vault.Locked += (_, _) => MainThread.BeginInvokeOnMainThread(async () =>
        {
            await GoToAsync(vault.IsInitialized ? "//unlock" : "//setup");
        });
    }

#if WINDOWS
    private VaultPage? vaultPage;

    /// <summary>
    /// Windows: the actions of the list stay in the title bar on every page of the open vault
    /// (list, settings, entry) and leave it when the vault is locked.
    /// </summary>
    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);

        Page? page = CurrentPage;
        if (page is VaultPage list)
            vaultPage = list;

        if (page is VaultPage or SettingsPage or EntryPage)
            vaultPage?.ShowTitleBarActions(page);
        else if (page is not null)
            vaultPage?.HideTitleBarActions(page);
    }
#elif ANDROID
    /// <summary>Android: the status bar follows the page shown and the theme (see <see cref="SystemBars"/>).</summary>
    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        ApplySystemBars();
    }

    private void ApplySystemBars()
    {
        SystemBars.Apply(CurrentPage);
        // The header of a page that has just been opened is created a little later.
        Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(250), () => SystemBars.Apply(CurrentPage));
    }
#endif
}
