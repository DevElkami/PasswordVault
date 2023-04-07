using MauiPasswordVault.ViewModel;

namespace MauiPasswordVault.View;

public partial class InitPage : ContentPage
{
    public InitPage(InitViewModel initViewModel)
    {
        BindingContext = initViewModel;
        InitializeComponent();
    }
}