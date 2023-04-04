using MauiPasswordVault.ViewModel;

namespace MauiPasswordVault.View;

public partial class ErrorPage : ContentPage
{
	public ErrorPage(ErrorViewModel errorViewModel)
	{
        BindingContext = errorViewModel;
        InitializeComponent();
	}
}