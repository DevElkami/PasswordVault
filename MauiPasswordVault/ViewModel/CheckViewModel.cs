using MauiPasswordVault.Service;
using MauiPasswordVault.View;
using Microsoft.Extensions.Logging;
using System.Windows.Input;
using VaultCore;

namespace MauiPasswordVault.ViewModel;

public class CheckViewModel
{
    private readonly NavigationService navigationService;
    private readonly ErrorService errorService;
    private readonly MyVault vault;
    private readonly ILogger<CheckViewModel> logger;
    private String vaultKey = null!;

    public CheckViewModel(NavigationService navigationService, ErrorService errorService, MyVault vault, ILogger<CheckViewModel> logger)
    {
        this.navigationService = navigationService;
        this.vault = vault;
        this.errorService = errorService;
        this.logger = logger;

        CheckCommand = new Command(
            execute: () =>
            {
                try
                {
                    if (vault.CheckVaultKey(VaultKey))
                        this.navigationService.NavigateBack();
                    else
                    {
                        this.errorService.LastErrorMessage = "Bad password vault";
                        this.errorService.CriticalError = false;
                        this.navigationService.NavigateToPage<ErrorPage>();
                    }
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, nameof(CheckCommand) + " failed");
                    this.errorService.LastErrorMessage = exception.Message;
                    this.errorService.LastErrorFull = exception.ToString();
                    this.errorService.CriticalError = true;
                    this.navigationService.NavigateToPage<ErrorPage>();
                }
            },
            canExecute: () =>
            {
                try
                {
                    return !String.IsNullOrEmpty(VaultKey) && (VaultKey.Length > 5);
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, nameof(CheckCommand) + " failed");
                    this.errorService.LastErrorMessage = exception.Message;
                    this.errorService.LastErrorFull = exception.ToString();
                    this.errorService.CriticalError = true;
                    this.navigationService.NavigateToPage<ErrorPage>();
                }

                return false;
            });
    }

    public bool IsInitialized() => vault.IsInitialized();

    public String VaultKey
    {
        get { return vaultKey; }
        set
        {
            vaultKey = value;
            (CheckCommand as Command)?.ChangeCanExecute();
        }
    }

    public ICommand CheckCommand { private set; get; }
}
