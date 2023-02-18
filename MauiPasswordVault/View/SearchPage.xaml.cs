using MauiPasswordVault.ViewModel;

namespace MauiPasswordVault.View;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchViewModel searchViewModel)
    {
        BindingContext = searchViewModel;
        InitializeComponent();
    }
}