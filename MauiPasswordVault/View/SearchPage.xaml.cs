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
            bool isInitialized = ((SearchViewModel)BindingContext).IsInitialized();
            if (!isInitialized)
                await navigationService.NavigateToPage<InitPage>();

            if (isInitialized && !((SearchViewModel)BindingContext).IsUnlock())
                await navigationService.NavigateToPage<CheckPage>();
        }
        catch (Exception exception)
        {
            this.errorService.LastErrorMessage = exception.Message;
            this.errorService.LastErrorFull = exception.ToString();
            this.errorService.CriticalError = true;
            await this.navigationService.NavigateToPage<ErrorPage>();
        }
    }
}