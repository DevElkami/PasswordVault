using MauiPasswordVault.Service;
using System.Windows.Input;
using VaultCore;

namespace MauiPasswordVault.ViewModel;

public class InitViewModel
{
    private readonly NavigationService navigationService;
    private readonly MyVault vault;
    private String newKey = null!;

    public InitViewModel(NavigationService navigationService, MyVault vault)
    {
        this.navigationService = navigationService;
        this.vault = vault;

        InitCommand = new Command(
            execute: () =>
            {
                if(vault.Initialize(NewVaultKey))
                    navigationService.NavigateBack(); 
                /*else              TODO                      
                    Toast.Make("Error: can't initialize app", ToastDuration.Short, 14).Show(new CancellationTokenSource().Token);  */              
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
