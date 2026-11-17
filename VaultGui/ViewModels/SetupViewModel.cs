using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VaultGui.Resources.Strings;
using VaultGui.Services;
using Microsoft.Extensions.Logging;
using VaultCore;
using VaultCore.Crypto;

namespace VaultGui.ViewModels;

/// <summary>
/// First launch: create a new vault, or download the one published on the user's web server
/// (a 3.x file is converted).
/// </summary>
public partial class SetupViewModel(IVaultService vault, VaultUpdateService update, AppSettings settings,
    IDialogService dialogs, ILogger<SetupViewModel> logger)
    : ViewModelBase(dialogs, logger)
{
    /// <summary>Downloaded 3.x file waiting for its password.</summary>
    private byte[]? legacyContent;

    /// <summary>Same setting as 3.x ("UPDATE_URL"): the address used before is already filled in.</summary>
    [ObservableProperty]
    public partial string DownloadUrl { get; set; } = settings.DownloadUrl;

    [ObservableProperty]
    public partial bool NeedsLegacyPassword { get; set; }

    [ObservableProperty]
    public partial string LegacyPassword { get; set; } = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasDownloadError))]
    public partial string DownloadError { get; set; } = "";

    public bool HasDownloadError => DownloadError.Length > 0;

    [RelayCommand]
    private Task DownloadAsync() => RunAsync(async () =>
    {
        DownloadError = "";
        NeedsLegacyPassword = false;
        legacyContent = null;
        if (DownloadUrl.Trim().Length == 0)
            return;

        settings.DownloadUrl = DownloadUrl.Trim();
        byte[] content = await VaultDownloader.DownloadAsync(settings.DownloadUrl);

        if (VaultCipher.IsVaultFile(content))
        {
            await Task.Run(() => vault.ImportVaultFile(content));
            update.MarkDownloaded(content);
            await GoToAsync("//unlock");
        }
        else if (VaultService.IsLegacyDataFile(content))
        {
            legacyContent = content;
            NeedsLegacyPassword = true;
        }
        else
        {
            DownloadError = Loc.Get(Loc.NotAVaultFile);
        }
    }, nameof(DownloadAsync));

    [RelayCommand]
    private Task OpenLegacyAsync() => RunAsync(async () =>
    {
        if (legacyContent is null || LegacyPassword.Length == 0)
            return;

        if (!await vault.CreateFromLegacyDataAsync(legacyContent, LegacyPassword))
        {
            DownloadError = Loc.Get(Loc.WrongPassword);
            return;
        }

        legacyContent = null;
        LegacyPassword = DownloadError = "";
        NeedsLegacyPassword = false;
        await GoToAsync("//vault");
    }, nameof(OpenLegacyAsync));

    public const int MinimumPasswordLength = 12;

    [ObservableProperty]
    public partial string Password { get; set; } = "";

    public PasswordStrengthIndicator Strength { get; } = new();

    partial void OnPasswordChanged(string value) => Strength.Update(value);

    [ObservableProperty]
    public partial string Confirm { get; set; } = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    public partial string ErrorMessage { get; set; } = "";

    public bool HasError => ErrorMessage.Length > 0;

    [RelayCommand]
    private Task CreateAsync() => RunAsync(async () =>
    {
        if (Password.Length < MinimumPasswordLength)
        {
            ErrorMessage = Loc.Format(Loc.PasswordTooShort, MinimumPasswordLength);
            return;
        }
        if (Password != Confirm)
        {
            ErrorMessage = Loc.Get(Loc.PasswordsDiffer);
            return;
        }

        ErrorMessage = "";
        await vault.CreateAsync(Password);
        Password = Confirm = "";
        await GoToAsync("//vault");
    }, nameof(CreateAsync));
}
