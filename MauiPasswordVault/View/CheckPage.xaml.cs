using MauiPasswordVault.Service;
using MauiPasswordVault.ViewModel;

namespace MauiPasswordVault.View;

public partial class CheckPage : ContentPage
{
    private readonly NavigationService navigationService;
    public CheckPage(NavigationService navigationService, CheckViewModel checkViewModel)
    {
        this.navigationService = navigationService;
        BindingContext = checkViewModel;
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        if (!((CheckViewModel)BindingContext).IsInitialized())
            await navigationService.NavigateToPage<InitPage>();
    }
}