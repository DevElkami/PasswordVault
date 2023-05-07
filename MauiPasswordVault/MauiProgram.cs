using CommunityToolkit.Maui;
using MauiPasswordVault.Service;
using MauiPasswordVault.View;
using MauiPasswordVault.ViewModel;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog;
using VaultCore;
using NLog.Config;
using NLog.Targets;

namespace MauiPasswordVault;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("AwesomeBrandsRegular.otf", "AwesomeBrands");
                fonts.AddFont("AwesomeRegular.otf", "AwesomeRegular");
                fonts.AddFont("AwesomeSolid.otf", "AwesomeSolid");
            });

        #region Logger
        LoggingConfiguration logConfig = new();

        FileTarget fileTarget = new()
        {
            FileName = typeof(MauiProgram).FullName + ".log",
            Layout = @"${longdate} [${uppercase:${level}}] ${logger} - ${message} ${exception:format=tostring}"
        };

        LoggingRule fileRule = new("*", NLog.LogLevel.Info, fileTarget);
        logConfig.LoggingRules.Add(fileRule);
        logConfig.AddTarget("logfile", fileTarget);

        builder.Logging.ClearProviders();
        builder.Logging.AddNLog(logConfig);
#if DEBUG
        builder.Logging.AddDebug();
#endif                
        #endregion

        builder.Services.AddSingleton<MyVault>();
        builder.Services.AddSingleton<NavigationService>();
        builder.Services.AddSingleton<ErrorService>();

        builder.Services.AddTransient<InitPage>();
        builder.Services.AddTransient<InitViewModel>();

        builder.Services.AddTransient<CheckPage>();
        builder.Services.AddTransient<CheckViewModel>();

        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddTransient<SearchViewModel>();

        builder.Services.AddTransient<ErrorPage>();
        builder.Services.AddTransient<ErrorViewModel>();

        return builder.Build();
    }
}