using MauiPasswordVault.Service;
using MauiPasswordVault.ViewModel;

namespace MauiPasswordVault.View;

public partial class InitPage : ContentPage
{
    private readonly NavigationService navigationService;
    public InitPage(NavigationService navigationService, InitViewModel initViewModel)
    {
        this.navigationService = navigationService;
        BindingContext = initViewModel;
        InitializeComponent();
    }
}