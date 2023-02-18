using MauiPasswordVault.View;

namespace MauiPasswordVault;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(InitPage), typeof(InitPage));
        Routing.RegisterRoute(nameof(CheckPage), typeof(CheckPage));
    }
}