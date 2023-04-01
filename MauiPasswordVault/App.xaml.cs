using MauiPasswordVault.View;

namespace MauiPasswordVault
{
    public partial class App : Application
    {
        public App(SearchPage searchPage)
        {
            InitializeComponent();

            MainPage = new NavigationPage(searchPage);            
        }
    }
}