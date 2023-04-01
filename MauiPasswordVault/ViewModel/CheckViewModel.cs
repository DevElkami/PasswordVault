using MauiPasswordVault.Service;
using System.Windows.Input;
using VaultCore;

namespace MauiPasswordVault.ViewModel;

public class CheckViewModel
{
    private readonly NavigationService navigationService;
    private readonly MyVault vault;
    private String vaultKey = null!;

    public CheckViewModel(NavigationService navigationService, MyVault vault)
    {
        this.navigationService = navigationService;
        this.vault = vault;

        CheckCommand = new Command(
            execute: () =>
            {
                if(vault.CheckVaultKey(VaultKey))
                    navigationService.NavigateBack();
                /*else TODO
                    Toast.Make("Bad passord", ToastDuration.Short, 14).Show(new CancellationTokenSource().Token);*/
            },
            canExecute: () =>
            {
                return !String.IsNullOrEmpty(VaultKey) && (VaultKey.Length > 5);
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
