using MauiPasswordVault.Service;
using MauiPasswordVault.ViewModel;

namespace MauiPasswordVault.View;

public partial class SearchPage : ContentPage
{
    private readonly NavigationService navigationService;
    private readonly ErrorService errorService;

    public SearchPage(NavigationService navigationService, ErrorService errorService, SearchViewModel searchViewModel)
    {
        this.navigationService = navigationService;
        this.errorService = errorService;
        BindingContext = searchViewModel;
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        try
        {
            if (!((SearchViewModel)BindingContext).IsInitialized())
            {
                await navigationService.NavigateToPage<InitPage>();
                return;
            }
            else
            {
                if (!((SearchViewModel)BindingContext).IsUnlock())
                    await navigationService.NavigateToPage<CheckPage>();
            }
        }
        catch (Exception exception)
        {
            this.errorService.LastErrorMessage = exception.Message;
            this.errorService.LastErrorFull = exception.ToString();
            this.errorService.CriticalError = true;
            await this.navigationService.NavigateToPage<ErrorPage>();
        }
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
        ((SearchViewModel)BindingContext).LoadAsync();
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
    }
}