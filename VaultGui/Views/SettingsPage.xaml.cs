using VaultGui.ViewModels;

namespace VaultGui.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel viewModel;

    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();

#if WINDOWS
        // Windows: no header band with a back arrow, the title bar keeps the actions of the list.
        Shell.SetNavBarIsVisible(this, false);
        // Nor the back arrow that Windows otherwise keeps at the left of the title bar.
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsVisible = false });
#endif
        BindingContext = this.viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }
}
