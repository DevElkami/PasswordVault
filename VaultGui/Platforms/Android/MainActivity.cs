using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace VaultGui;

[Activity(Theme = "@style/MyAppTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Sensor, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Passwords on screen: block screenshots, screen recording and the recent-apps thumbnail.
        Window?.SetFlags(WindowManagerFlags.Secure, WindowManagerFlags.Secure);

        // No autofill anywhere in the app: the system (Google Password Manager...) must neither offer to fill
        // the master password nor to save what is typed here.
        if (Window?.DecorView is Android.Views.View root)
            root.ImportantForAutofill = ImportantForAutofill.NoExcludeDescendants;
    }
}
