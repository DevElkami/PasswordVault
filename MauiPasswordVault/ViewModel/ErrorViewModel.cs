using MauiPasswordVault.Service;
using System.Windows.Input;

namespace MauiPasswordVault.ViewModel;

public class ErrorViewModel
{
    private readonly NavigationService navigationService;
    private readonly ErrorService errorService;

    public ErrorViewModel(NavigationService navigationService, ErrorService errorService)
    {
        this.navigationService = navigationService;
        this.errorService = errorService;

        AcceptCommand = new Command(
            execute: () =>
            {
                if (errorService.CriticalError)
                    Application.Current?.Quit();
                else
                    this.navigationService.NavigateBack();
            },
            canExecute: () =>
            {
                return true;
            });
    }

    public String LastErrorMessage => errorService.LastErrorMessage;

    public ICommand AcceptCommand { private set; get; }
}
