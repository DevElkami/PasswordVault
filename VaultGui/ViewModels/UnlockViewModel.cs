using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VaultGui.Resources.Strings;
using VaultGui.Services;
using Microsoft.Extensions.Logging;
using VaultCore;

namespace VaultGui.ViewModels;

public partial class UnlockViewModel(IVaultService vault, QuickUnlockService quickUnlock, AppSettings settings, IDialogService dialogs, ILogger<UnlockViewModel> logger)
    : ViewModelBase(dialogs, logger)
{
    [ObservableProperty]
    public partial string Password { get; set; } = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    public partial string ErrorMessage { get; set; } = "";

    public bool HasError => ErrorMessage.Length > 0;

    [ObservableProperty]
    public partial bool NeedsMigration { get; set; }

    /// <summary>Biometric unlock is set up for this vault.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowLogo))]
    public partial bool CanQuickUnlock { get; set; }

    /// <summary>No quick unlock: the cube takes the place of the fingerprint button.</summary>
    public bool ShowLogo => !CanQuickUnlock;

    public string QuickUnlockText => Loc.Format(Loc.UnlockWith, quickUnlock.Name);

    /// <summary>Quick unlock was switched off automatically: say why the password is asked.</summary>
    [ObservableProperty]
    public partial bool QuickUnlockTurnedOff { get; set; }

    public async Task OnAppearingAsync()
    {
        Password = "";
        ErrorMessage = "";
        NeedsMigration = vault.NeedsMigration;
        CanQuickUnlock = quickUnlock.IsUsable;
        QuickUnlockTurnedOff = settings.QuickUnlockTurnedOff && !quickUnlock.IsEnabled;

        // Offer it right away: one tap (or glance) instead of typing the password.
        if (CanQuickUnlock && !vault.IsUnlocked)
            await QuickUnlockAsync();
    }

    [RelayCommand]
    private Task QuickUnlockAsync() => RunAsync(async () =>
    {
        if (await quickUnlock.UnlockAsync())
        {
            await OnUnlockedAsync();
            return;
        }

        CanQuickUnlock = quickUnlock.IsUsable;
        QuickUnlockTurnedOff = settings.QuickUnlockTurnedOff && !quickUnlock.IsEnabled;
    }, nameof(QuickUnlockAsync));

    private async Task OnUnlockedAsync()
    {
        Password = "";
        ErrorMessage = "";
        await GoToAsync("//vault");
    }

    [RelayCommand]
    private Task UnlockAsync() => RunAsync(async () =>
    {
        if (Password.Length == 0)
            return;

        if (!await vault.UnlockAsync(Password))
        {
            ErrorMessage = Loc.Get(Loc.WrongPassword);
            return;
        }

        await OnUnlockedAsync();
        await OfferQuickUnlockAsync();
    }, nameof(UnlockAsync));

    /// <summary>
    /// Where quick unlock exists (Android biometrics, Windows Hello), ask once after a password unlock whether to use them next time.
    /// Later it stays a switch in Settings.
    /// </summary>
    private async Task OfferQuickUnlockAsync()
    {
        if (settings.QuickUnlockOffered || quickUnlock.IsEnabled || !await quickUnlock.IsAvailableAsync())
            return;

        settings.QuickUnlockOffered = true;
        if (await Dialogs.ConfirmAsync(Loc.Format(Loc.QuickUnlockOffer, quickUnlock.Name)))
            await quickUnlock.EnableAsync();
    }
}
