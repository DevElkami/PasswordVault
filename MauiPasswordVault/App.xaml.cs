using MauiPasswordVault.View;

namespace MauiPasswordVault
{
    public partial class App : Application
    {
        public App(SearchPage searchPage)
        {
            if (Application.Current != null)
                Application.Current.UserAppTheme = AppTheme.Dark;

            InitializeComponent();

            MainPage = new NavigationPage(searchPage);
        }
    }
}