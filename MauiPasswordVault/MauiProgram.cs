using MauiPasswordVault.Service;
using MauiPasswordVault.View;
using MauiPasswordVault.ViewModel;
using Microsoft.Extensions.Logging;
using VaultCore;

namespace MauiPasswordVault;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("AwesomeBrandsRegular.otf", "AwesomeBrands");
                fonts.AddFont("AwesomeRegular.otf", "AwesomeRegular");
                fonts.AddFont("AwesomeSolid.otf", "AwesomeSolid");
            });

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
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}