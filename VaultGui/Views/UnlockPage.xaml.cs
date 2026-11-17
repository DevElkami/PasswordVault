using VaultGui.ViewModels;

namespace VaultGui.Views;

public partial class UnlockPage : ContentPage
{
    private readonly UnlockViewModel viewModel;

    public UnlockPage(UnlockViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.OnAppearingAsync();

        // Biometrics first: the keyboard only comes up by itself where the password is the only way in.
        if (!viewModel.CanQuickUnlock)
            PasswordEntry.FocusInput();
    }
}
