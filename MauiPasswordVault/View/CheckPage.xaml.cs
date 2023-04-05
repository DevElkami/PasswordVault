using MauiPasswordVault.Service;
using MauiPasswordVault.ViewModel;

namespace MauiPasswordVault.View;

public partial class CheckPage : ContentPage
{
    private readonly NavigationService navigationService;
    private readonly ErrorService errorService;

    public CheckPage(NavigationService navigationService, ErrorService errorService, CheckViewModel checkViewModel)
    {
        this.navigationService = navigationService;
        this.errorService = errorService;
        BindingContext = checkViewModel;
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        try
        {
            if (!((CheckViewModel)BindingContext).IsInitialized())
                await navigationService.NavigateToPage<InitPage>();
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