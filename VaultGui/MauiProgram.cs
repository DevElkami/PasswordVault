using VaultGui.Services;
using VaultGui.ViewModels;
using VaultGui.Views;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using VaultCore;

namespace VaultGui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        ConfigureLogging(builder);

        // Vault
        VaultOptions vaultOptions = VaultOptions.CreateDefault();
        builder.Services.AddSingleton(vaultOptions);
        builder.Services.AddSingleton<IVaultService>(new VaultService(vaultOptions));

        // App services
        builder.Services.AddSingleton<AppSettings>();
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<ISecureClipboard, SecureClipboard>();
        builder.Services.AddSingleton<AutoLockService>();
        builder.Services.AddSingleton<VaultUpdateService>();
        builder.Services.AddSingleton<QuickUnlockService>();
        builder.Services.AddSingleton<UploadService>();
#if WINDOWS
        builder.Services.AddSingleton<IBiometricKeyProtector, Platforms.Windows.WindowsHelloKeyProtector>();
        builder.Services.AddSingleton<IVaultUploader, Platforms.Windows.Upload.WindowsVaultUploader>();
#elif ANDROID
        builder.Services.AddSingleton<IBiometricKeyProtector, Platforms.Android.AndroidBiometricKeyProtector>();
        builder.Services.AddSingleton<IVaultUploader, NoVaultUploader>();
#else
        builder.Services.AddSingleton<IBiometricKeyProtector, NoBiometricKeyProtector>();
        builder.Services.AddSingleton<IVaultUploader, NoVaultUploader>();
#endif
        builder.Services.AddSingleton<AppShell>();

        // Pages and view models
        builder.Services.AddTransient<SetupPage>().AddTransient<SetupViewModel>();
        builder.Services.AddTransient<UnlockPage>().AddTransient<UnlockViewModel>();
        builder.Services.AddTransient<VaultPage>().AddTransient<VaultViewModel>();
        builder.Services.AddTransient<EntryPage>().AddTransient<EntryViewModel>();
        builder.Services.AddTransient<SettingsPage>().AddTransient<SettingsViewModel>();

        return builder.Build();
    }

    /// <summary>
    /// Warnings and errors only, in the app data folder. Never log vault content.
    /// </summary>
    private static void ConfigureLogging(MauiAppBuilder builder)
    {
        FileTarget file = new("logfile")
        {
            FileName = Path.Combine(FileSystem.AppDataDirectory, "logs", "PasswordVault.log"),
            Layout = "${longdate} [${uppercase:${level}}] ${logger} - ${message} ${exception:format=tostring}",
            ArchiveAboveSize = 1_000_000,
            MaxArchiveFiles = 2,
        };

        LoggingConfiguration config = new();
        config.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Fatal, file);

        builder.Logging.ClearProviders();
        builder.Logging.AddNLog(config);
#if DEBUG
        builder.Logging.AddDebug();
#endif
    }
}
