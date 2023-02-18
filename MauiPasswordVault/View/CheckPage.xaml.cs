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
        https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation?view=net-maui-7.0
        if (!((CheckViewModel)BindingContext).IsInitialized())
            await Shell.Current.GoToAsync(nameof(InitPage));
    }
}