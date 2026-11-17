using Android.Views;
using AndroidX.Core.View;
using Google.Android.Material.AppBar;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;

namespace VaultGui;

/// <summary>
/// Android draws the app under the status bar. Keeps that strip consistent with the page:
/// same colour as the header (light or dark theme), and dark status icons on the pages
/// that have no header and a light background (setup, unlock).
/// </summary>
internal static class SystemBars
{
    public static void Apply(Page? page)
    {
        if (page is null || Platform.CurrentActivity?.Window is not { } window || Application.Current is not { } app)
            return;

        bool dark = app.RequestedTheme == AppTheme.Dark;
        bool hasHeader = Shell.GetNavBarIsVisible(page);

        // The strip behind the status bar belongs to the header: give it the colour of the header.
        if (app.Resources.TryGetValue(dark ? "NightBar" : "Primary", out object value) && value is Color header)
            Paint(window.DecorView, header.ToPlatform());

        // White icons on the coloured header and on dark pages, dark icons on a light page without header.
        WindowInsetsControllerCompat? controller = WindowCompat.GetInsetsController(window, window.DecorView);
        if (controller is not null)
            controller.AppearanceLightStatusBars = !hasHeader && !dark;
    }

    private static void Paint(AView view, Android.Graphics.Color color)
    {
        if (view is AppBarLayout appBar)
        {
            appBar.SetBackgroundColor(color);
            return;
        }

        if (view is ViewGroup group)
        {
            for (int i = 0; i < group.ChildCount; i++)
            {
                if (group.GetChildAt(i) is { } child)
                    Paint(child, color);
            }
        }
    }
}
