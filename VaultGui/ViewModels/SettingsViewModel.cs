using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VaultGui.Resources.Strings;
using VaultGui.Services;
using Microsoft.Extensions.Logging;
using VaultCore;
using VaultCore.Import;
using VaultCore.Models;

namespace VaultGui.ViewModels;

public sealed record AutoLockChoice(int Minutes)
{
    public override string ToString() =>
        Minutes == 0 ? Loc.Get(Loc.Immediately) : Loc.Format(Loc.Minutes, Minutes);
}

public sealed record ThemeChoice(AppTheme Theme)
{
    public override string ToString() => Loc.Get(Theme switch
    {
        AppTheme.Light => Loc.ThemeLight,
        AppTheme.Dark => Loc.ThemeDark,
        _ => Loc.ThemeSystem,
    });
}

public partial class SettingsViewModel : ViewModelBase
{
    private readonly IVaultService vault;
    private readonly VaultOptions options;
    private readonly AppSettings settings;
    private readonly VaultUpdateService update;
    private readonly QuickUnlockService quickUnlock;
    private readonly UploadService upload;

    public SettingsViewModel(IVaultService vault, VaultOptions options, AppSettings settings, VaultUpdateService update,
        QuickUnlockService quickUnlock, UploadService upload, IDialogService dialogs, ILogger<SettingsViewModel> logger)
        : base(dialogs, logger)
    {
        this.vault = vault;
        this.options = options;
        this.settings = settings;
        this.update = update;
        this.quickUnlock = quickUnlock;
        this.upload = upload;
        UploadAddress = upload.Address;
        UploadUser = upload.User;
        QuickUnlockEnabled = quickUnlock.IsEnabled;

        DownloadUrl = settings.DownloadUrl;
        SelectedAutoLock = AutoLockChoices.FirstOrDefault(c => c.Minutes == settings.AutoLockMinutes) ?? AutoLockChoices[1];
        SelectedTheme = ThemeChoices.FirstOrDefault(c => c.Theme == settings.Theme) ?? ThemeChoices[0];
    }

    public IReadOnlyList<AutoLockChoice> AutoLockChoices { get; } = new AutoLockChoice[] { new(0), new(1), new(5), new(15) };

    public IReadOnlyList<ThemeChoice> ThemeChoices { get; } = new ThemeChoice[] { new(AppTheme.Unspecified), new(AppTheme.Light), new(AppTheme.Dark) };

    [ObservableProperty]
    public partial ThemeChoice SelectedTheme { get; set; }

    /// <summary>Applied at once, and remembered for the next start.</summary>
    partial void OnSelectedThemeChanged(ThemeChoice value)
    {
        settings.Theme = value.Theme;
        if (Application.Current is Application app)
            app.UserAppTheme = value.Theme;
    }

    public string VaultLocation => Loc.Format(Loc.VaultLocation, options.VaultFilePath);

    public string Version => Loc.Format(Loc.Version, AppInfo.Current.VersionString);

    [ObservableProperty]
    public partial AutoLockChoice SelectedAutoLock { get; set; }

    partial void OnSelectedAutoLockChanged(AutoLockChoice value) => settings.AutoLockMinutes = value.Minutes;

    [ObservableProperty]
    public partial string DownloadUrl { get; set; } = "";

    public async void OnAppearing()
    {
        IsQuickUnlockAvailable = await quickUnlock.IsAvailableAsync();
        await RefreshUploadAsync();
    }

    #region Send to server
    public bool IsUploadSupported => upload.IsSupported;

    [ObservableProperty]
    public partial string UploadAddress { get; set; } = "";

    [ObservableProperty]
    public partial string UploadUser { get; set; } = "";

    /// <summary>Only what the user types now: the stored password is never shown.</summary>
    [ObservableProperty]
    public partial string UploadPassword { get; set; } = "";

    [ObservableProperty]
    public partial string UploadPasswordPlaceholder { get; set; } = Loc.Get(Loc.UploadPassword);

    [ObservableProperty]
    public partial string UploadStatus { get; set; } = "";

    [ObservableProperty]
    public partial bool IsUploadConfigured { get; set; }

    private async Task RefreshUploadAsync()
    {
        IsUploadConfigured = upload.IsConfigured;
        UploadStatus = upload.StatusText;
        UploadPasswordPlaceholder = Loc.Get(await upload.HasStoredPasswordAsync() ? Loc.UploadPasswordStored : Loc.UploadPassword);
    }

    [RelayCommand]
    private Task UploadAsync() => RunAsync(async () =>
    {
        try
        {
            await upload.ConfigureAsync(UploadAddress, UploadUser, UploadPassword);
        }
        catch (FormatException)
        {
            await Dialogs.AlertAsync(Loc.Get(Loc.UploadBadAddress));
            return;
        }

        UploadPassword = "";
        try
        {
            if (await upload.SendAsync())
                await Dialogs.AlertAsync(Loc.Get(Loc.UploadDone));
        }
        finally
        {
            await RefreshUploadAsync();
        }
    }, nameof(UploadAsync));

    /// <summary>Take the file off the server (less exposure while the other devices do not need it).</summary>
    [RelayCommand]
    private Task UploadDeleteAsync() => RunAsync(async () =>
    {
        if (!await Dialogs.ConfirmAsync(Loc.Format(Loc.UploadConfirmDelete, upload.Address)))
            return;

        try
        {
            if (await upload.DeleteRemoteAsync())
                await Dialogs.AlertAsync(Loc.Get(Loc.UploadDeleted));
        }
        finally
        {
            await RefreshUploadAsync();
        }
    }, nameof(UploadDeleteAsync));

    [RelayCommand]
    private Task UploadForgetAsync() => RunAsync(async () =>
    {
        if (!await Dialogs.ConfirmAsync(Loc.Get(Loc.UploadConfirmForget)))
            return;

        upload.Forget();
        UploadAddress = UploadUser = UploadPassword = "";
        await RefreshUploadAsync();
    }, nameof(UploadForgetAsync));
    #endregion

    #region Quick unlock
    [ObservableProperty]
    public partial bool IsQuickUnlockAvailable { get; set; }

    public string QuickUnlockText => Loc.Format(Loc.QuickUnlockSetting, quickUnlock.Name);

    [ObservableProperty]
    public partial bool QuickUnlockEnabled { get; set; }

    /// <summary>Set while the switch is moved back by code (cancelled check), so it does not act again.</summary>
    private bool updatingQuickUnlock;

    partial void OnQuickUnlockEnabledChanged(bool value)
    {
        if (!updatingQuickUnlock && value != quickUnlock.IsEnabled)
            _ = ToggleQuickUnlockAsync(value);
    }

    private async Task ToggleQuickUnlockAsync(bool enable)
    {
        await RunAsync(async () =>
        {
            if (enable)
                await quickUnlock.EnableAsync();
            else
                quickUnlock.Disable();
        }, nameof(ToggleQuickUnlockAsync));

        // Cancelled or failed: the switch shows the real state.
        SetQuickUnlockSwitch(quickUnlock.IsEnabled);
    }

    private void SetQuickUnlockSwitch(bool value)
    {
        updatingQuickUnlock = true;
        QuickUnlockEnabled = value;
        updatingQuickUnlock = false;
    }

    /// <summary>After a password change, quick unlock is off: offer to turn it back on while we are here.</summary>
    private async Task OfferQuickUnlockAgainAsync()
    {
        bool wasOn = QuickUnlockEnabled;
        SetQuickUnlockSwitch(quickUnlock.IsEnabled);
        if (wasOn && !quickUnlock.IsEnabled && await Dialogs.ConfirmAsync(Loc.Format(Loc.QuickUnlockReenable, quickUnlock.Name)))
            SetQuickUnlockSwitch(await quickUnlock.EnableAsync());
    }
    #endregion

    [ObservableProperty]
    public partial string NewPassword { get; set; } = "";

    public PasswordStrengthIndicator Strength { get; } = new();

    partial void OnNewPasswordChanged(string value) => Strength.Update(value);

    [ObservableProperty]
    public partial string ConfirmNewPassword { get; set; } = "";

    [RelayCommand]
    private Task ImportCsvAsync() => RunAsync(async () =>
    {
        List<MyPassword> found;
        await using (Stream? stream = await VaultFilePicker.PickCsvAsync())
        {
            if (stream is null)
                return;
            found = await Task.Run(() => CsvPasswordImporter.Read(stream));
        }

        if (!await Dialogs.ConfirmAsync(Loc.Format(Loc.ImportConfirm, found.Count)))
            return;

        int added = found.Count(vault.Add);
        await vault.SaveAsync();
        await Dialogs.AlertAsync(Loc.Format(Loc.Imported, added));
    }, nameof(ImportCsvAsync));

    /// <summary>Master password of the 3.x file to import (not the one of this vault).</summary>
    [ObservableProperty]
    public partial string LegacyPassword { get; set; } = "";

    [RelayCommand]
    private Task ImportLegacyAsync() => RunAsync(async () =>
    {
        if (LegacyPassword.Length == 0)
        {
            await Dialogs.AlertAsync(Loc.Get(Loc.ImportLegacyNoPassword));
            return;
        }

        if (await VaultFilePicker.PickLegacyAsync() is not byte[] content)
            return;

        if (!VaultService.IsLegacyDataFile(content))
        {
            await Dialogs.AlertAsync(Loc.Get(Loc.ImportLegacyBadFile));
            return;
        }

        if (await vault.ImportLegacyDataAsync(content, LegacyPassword) is not int added)
        {
            await Dialogs.AlertAsync(Loc.Get(Loc.WrongPassword));
            return;
        }

        LegacyPassword = "";
        await Dialogs.AlertAsync(Loc.Format(Loc.Imported, added));
    }, nameof(ImportLegacyAsync));

    [RelayCommand]
    private Task DownloadAsync() => RunAsync(async () =>
    {
        // Remembered: afterwards the main screen has an "Update" button.
        update.Remember(DownloadUrl);
        if (!update.IsConfigured)
            return;

        if (VaultUpdateService.Message(await update.UpdateAsync()) is string message)
            await Dialogs.AlertAsync(message);
    }, nameof(DownloadAsync));

    [RelayCommand]
    private Task ChangePasswordAsync() => RunAsync(async () =>
    {
        if (NewPassword.Length < SetupViewModel.MinimumPasswordLength)
        {
            await Dialogs.AlertAsync(Loc.Format(Loc.PasswordTooShort, SetupViewModel.MinimumPasswordLength));
            return;
        }
        if (NewPassword != ConfirmNewPassword)
        {
            await Dialogs.AlertAsync(Loc.Get(Loc.PasswordsDiffer));
            return;
        }

        await vault.ChangePasswordAsync(NewPassword);
        NewPassword = ConfirmNewPassword = "";
        await Dialogs.AlertAsync(Loc.Get(Loc.PasswordChanged));
        await OfferQuickUnlockAgainAsync();
    }, nameof(ChangePasswordAsync));
}
