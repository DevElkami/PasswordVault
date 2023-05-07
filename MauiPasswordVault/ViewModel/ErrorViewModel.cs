using MauiPasswordVault.Service;
using Microsoft.Extensions.Logging;
using System.Windows.Input;

namespace MauiPasswordVault.ViewModel;

public class ErrorViewModel
{
    private readonly NavigationService navigationService;
    private readonly ErrorService errorService;
    private readonly ILogger<ErrorViewModel> logger;

    public ErrorViewModel(NavigationService navigationService, ErrorService errorService, ILogger<ErrorViewModel> logger)
    {
        this.navigationService = navigationService;
        this.errorService = errorService;
        this.logger = logger;

        AcceptCommand = new Command(
            execute: () =>
            {
                try
                {
                    if (errorService.CriticalError)
                        Application.Current?.Quit();
                    else
                        this.navigationService.NavigateBack();
                }
                catch (Exception except)
                {
                    logger.LogCritical(except, nameof(AcceptCommand) + " failed");
                }
            },
            canExecute: () =>
            {
                return true;
            });
    }

    public String LastErrorMessage => errorService.LastErrorMessage;

    public ICommand AcceptCommand { private set; get; }
}
