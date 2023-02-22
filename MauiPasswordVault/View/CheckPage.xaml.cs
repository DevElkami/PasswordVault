using MauiPasswordVault.ViewModel;

namespace MauiPasswordVault.View;

public partial class CheckPage : ContentPage
{
	public CheckPage(CheckViewModel checkViewModel)
    {
        BindingContext = checkViewModel;
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        if (!((CheckViewModel)BindingContext).IsInitialized())            
            await Shell.Current.GoToAsync(nameof(InitPage));
    }
}