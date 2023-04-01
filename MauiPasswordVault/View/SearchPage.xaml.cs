using MauiPasswordVault.Service;
using MauiPasswordVault.ViewModel;

namespace MauiPasswordVault.View;

public partial class SearchPage : ContentPage
{
    private readonly NavigationService navigationService;
    public SearchPage(NavigationService navigationService, SearchViewModel searchViewModel)
    {
        this.navigationService = navigationService;
        BindingContext = searchViewModel;
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        bool isInitialized = ((SearchViewModel)BindingContext).IsInitialized();
        if (!isInitialized)
            await navigationService.NavigateToPage<InitPage>();

        if (isInitialized && !((SearchViewModel)BindingContext).IsUnlock())
            await navigationService.NavigateToPage<CheckPage>();
    }
}