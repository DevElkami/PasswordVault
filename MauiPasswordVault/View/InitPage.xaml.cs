using MauiPasswordVault.ViewModel;
using VaultCore;

namespace MauiPasswordVault.View;

public partial class InitPage : ContentPage
{
    int count = 0;

    public InitPage(InitViewModel initViewModel)
    {
        BindingContext = initViewModel;            
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}