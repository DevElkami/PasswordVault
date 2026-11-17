using System.Globalization;

namespace VaultGui.Resources.Strings;

/// <summary>
/// Localized strings for C# code. XAML uses {StaticResource Key} directly.
/// </summary>
public static class Loc
{
    public const string AppTitle = nameof(AppTitle);
    public const string Ok = nameof(Ok);
    public const string Cancel = nameof(Cancel);
    public const string Yes = nameof(Yes);
    public const string No = nameof(No);
    public const string Error = nameof(Error);
    public const string PasswordTooShort = nameof(PasswordTooShort);
    public const string PasswordsDiffer = nameof(PasswordsDiffer);
    public const string PasswordStrength = nameof(PasswordStrength);
    public const string StrengthVeryWeak = nameof(StrengthVeryWeak);
    public const string StrengthWeak = nameof(StrengthWeak);
    public const string StrengthFair = nameof(StrengthFair);
    public const string StrengthStrong = nameof(StrengthStrong);
    public const string StrengthVeryStrong = nameof(StrengthVeryStrong);
    public const string WrongPassword = nameof(WrongPassword);
    public const string Lock = nameof(Lock);
    public const string Settings = nameof(Settings);
    public const string NoEntries = nameof(NoEntries);
    public const string NoResults = nameof(NoResults);
    public const string Copied = nameof(Copied);
    public const string NewEntry = nameof(NewEntry);
    public const string EditEntry = nameof(EditEntry);
    public const string Show = nameof(Show);
    public const string Hide = nameof(Hide);
    public const string ConfirmDelete = nameof(ConfirmDelete);
    public const string Duplicate = nameof(Duplicate);
    public const string EntryEmpty = nameof(EntryEmpty);
    public const string ImportConfirm = nameof(ImportConfirm);
    public const string Imported = nameof(Imported);
    public const string VaultLocation = nameof(VaultLocation);
    public const string Downloaded = nameof(Downloaded);
    public const string DownloadedOther = nameof(DownloadedOther);
    public const string ConfirmReplace = nameof(ConfirmReplace);
    public const string PasswordChanged = nameof(PasswordChanged);
    public const string Immediately = nameof(Immediately);
    public const string Minutes = nameof(Minutes);
    public const string Version = nameof(Version);
    public const string Update = nameof(Update);
    public const string UpToDate = nameof(UpToDate);
    public const string UploadPassword = nameof(UploadPassword);
    public const string UploadPasswordStored = nameof(UploadPasswordStored);
    public const string UploadShort = nameof(UploadShort);
    public const string UploadDone = nameof(UploadDone);
    public const string UploadDoneAt = nameof(UploadDoneAt);
    public const string UploadPending = nameof(UploadPending);
    public const string UploadBadAddress = nameof(UploadBadAddress);
    public const string UploadMissing = nameof(UploadMissing);
    public const string UploadAuthFailed = nameof(UploadAuthFailed);
    public const string UploadNoTls = nameof(UploadNoTls);
    public const string UploadTrustTitle = nameof(UploadTrustTitle);
    public const string UploadTrustFirst = nameof(UploadTrustFirst);
    public const string UploadTrustChanged = nameof(UploadTrustChanged);
    public const string UploadConfirmForget = nameof(UploadConfirmForget);
    public const string UploadConfirmDelete = nameof(UploadConfirmDelete);
    public const string UploadDeleted = nameof(UploadDeleted);
    public const string UploadDeletedAt = nameof(UploadDeletedAt);
    public const string LegacyInSettings = nameof(LegacyInSettings);
    public const string NotAVaultFile = nameof(NotAVaultFile);
    public const string ConfirmDeleteEntry = nameof(ConfirmDeleteEntry);
    public const string Duplicates = nameof(Duplicates);
    public const string ShowAll = nameof(ShowAll);
    public const string NoDuplicates = nameof(NoDuplicates);
    public const string DuplicatesFound = nameof(DuplicatesFound);
    public const string ThemeSystem = nameof(ThemeSystem);
    public const string ThemeLight = nameof(ThemeLight);
    public const string ThemeDark = nameof(ThemeDark);
    public const string ImportLegacyNoPassword = nameof(ImportLegacyNoPassword);
    public const string ImportLegacyBadFile = nameof(ImportLegacyBadFile);
    public const string Biometrics = nameof(Biometrics);
    public const string QuickUnlockPrompt = nameof(QuickUnlockPrompt);
    public const string UnlockWith = nameof(UnlockWith);
    public const string QuickUnlockSetting = nameof(QuickUnlockSetting);
    public const string QuickUnlockReenable = nameof(QuickUnlockReenable);
    public const string QuickUnlockOffer = nameof(QuickUnlockOffer);

    /// <summary>French for any fr-* culture, English otherwise.</summary>
    public static ResourceDictionary ForCurrentCulture() =>
        CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "fr" ? new StringsFr() : new StringsEn();

    public static string Get(string key) =>
        Application.Current?.Resources.TryGetValue(key, out object? value) == true && value is string text ? text : key;

    public static string Format(string key, params object[] args) =>
        string.Format(CultureInfo.CurrentCulture, Get(key), args);
}
