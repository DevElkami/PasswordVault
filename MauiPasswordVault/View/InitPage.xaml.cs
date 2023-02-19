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

    private void OnCounterClicked(object sender, EventArgs e)
    {        
        //SemanticScreenReader.Announce(CounterBtn.Text);
    }
}