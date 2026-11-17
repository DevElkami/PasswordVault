using VaultGui.Resources.Strings;
using VaultGui.Services;

namespace VaultGui;

public partial class App : Application
{
    private readonly IServiceProvider services;

    public App(IServiceProvider services)
    {
        this.services = services;
        InitializeComponent();

        // Light / dark as chosen in Settings (default: the theme of the system).
        UserAppTheme = services.GetRequiredService<AppSettings>().Theme;

        // Strings must be in place before any page is created (pages use {StaticResource}).
        Resources.MergedDictionaries.Add(Loc.ForCurrentCulture());
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = new(services.GetRequiredService<AppShell>())
        {
            Title = Loc.Get(Loc.AppTitle),
        };

#if WINDOWS
        // The Windows title bar is the header of the app: cube, name, and the actions of the list
        // (added by VaultPage while it is shown).
        TitleBar titleBar = new()
        {
            Icon = "tb_cube.png",
            Title = Loc.Get(Loc.AppTitle),
            ForegroundColor = Colors.White,
        };
        // Bright purple on light, dark plum on dark.
        titleBar.SetAppThemeColor(TitleBar.BackgroundColorProperty, ThemeColor("Primary"), ThemeColor("NightBar"));
        window.TitleBar = titleBar;
        window.HandlerChanged += (_, _) =>
        {
            // Minimize / maximize / close in white, to stay readable on the coloured bar.
            if (window.Handler?.PlatformView is Microsoft.UI.Xaml.Window native)
            {
                Microsoft.UI.Windowing.AppWindowTitleBar buttons = native.AppWindow.TitleBar;
                buttons.ButtonForegroundColor = Microsoft.UI.Colors.White;
                buttons.ButtonHoverForegroundColor = Microsoft.UI.Colors.White;
                buttons.ButtonHoverBackgroundColor = global::Windows.UI.Color.FromArgb(48, 255, 255, 255);
                buttons.ButtonPressedForegroundColor = Microsoft.UI.Colors.White;
                buttons.ButtonPressedBackgroundColor = global::Windows.UI.Color.FromArgb(80, 255, 255, 255);
                buttons.ButtonInactiveForegroundColor = global::Windows.UI.Color.FromArgb(255, 223, 216, 247);
            }
        };
        window.Width = 540;
        window.Height = 860;
#endif

        services.GetRequiredService<AutoLockService>().Attach(window);
        return window;
    }

    private Color ThemeColor(string key) =>
        Resources.TryGetValue(key, out object value) && value is Color color ? color : Colors.Purple;
}
