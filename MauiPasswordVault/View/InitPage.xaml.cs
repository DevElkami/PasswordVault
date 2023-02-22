using MauiPasswordVault.ViewModel;
using VaultCore;

namespace MauiPasswordVault.View;

public partial class InitPage : ContentPage
{    
    public InitPage(InitViewModel initViewModel)
    {
        BindingContext = initViewModel;            
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {        
        //await Shell.Current.GoToAsync(nameof(InitPage));
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {        
        //SemanticScreenReader.Announce(CounterBtn.Text);
    }
}