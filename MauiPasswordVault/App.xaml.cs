using MauiPasswordVault.View;
using Microsoft.Maui.Hosting;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MauiPasswordVault
{
    public partial class App : Application
    {
        public App(SearchPage searchPage)
        {
            if (Application.Current != null)
                Application.Current.UserAppTheme = AppTheme.Dark;
            InitializeComponent();

#if DEBUG
            Resources.MergedDictionaries.Add(new MauiPasswordVault.Resources.Lang.en());
#else
            if (Thread.CurrentThread.CurrentUICulture.IetfLanguageTag == "fr-FR")
                Resources.MergedDictionaries.Add(new MauiPasswordVault.Resources.Lang.fr());
            else
                Resources.MergedDictionaries.Add(new MauiPasswordVault.Resources.Lang.en());
#endif

            MainPage = new NavigationPage(searchPage);
        }
    }
}