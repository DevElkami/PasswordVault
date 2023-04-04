using MauiPasswordVault.Service;
using MauiPasswordVault.View;
using System.Windows.Input;
using VaultCore;

namespace MauiPasswordVault.ViewModel;

public class InitViewModel
{
    private readonly NavigationService navigationService;
    private readonly MyVault vault;
    private readonly ErrorService errorService;
    private String newKey = null!;

    public InitViewModel(NavigationService navigationService, ErrorService errorService, MyVault vault)
    {
        this.navigationService = navigationService;
        this.errorService = errorService;
        this.vault = vault;

        InitCommand = new Command(
            execute: () =>
            {
                try
                {
                    if (this.vault.Initialize(NewVaultKey))
                        this.navigationService.NavigateBack();
                    else
                    {
                        this.errorService.LastErrorMessage = "Can't initialize vault";
                        this.errorService.CriticalError = false;
                        this.navigationService.NavigateToPage<ErrorPage>();
                    }
                }
                catch(Exception exception)
                {
                    this.errorService.LastErrorMessage = exception.Message;
                    this.errorService.LastErrorFull = exception.ToString();
                    this.errorService.CriticalError = true;
                    this.navigationService.NavigateToPage<ErrorPage>();
                }
            },
            canExecute: () =>
            {
                return !String.IsNullOrEmpty(NewVaultKey) && (NewVaultKey.Length > 5);
            });
    }

    public String NewVaultKey
    {
        get { return newKey; }
        set
        {
            newKey = value;
            (InitCommand as Command)?.ChangeCanExecute();
        }
    }

    public ICommand InitCommand { private set; get; }
}
