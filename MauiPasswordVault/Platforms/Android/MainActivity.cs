﻿using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MauiPasswordVault
{
    [Activity(Theme = "@style/MyAppTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Sensor, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
    }
}