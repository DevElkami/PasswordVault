using MauiPasswordVault.Service;
using MauiPasswordVault.View;
using System.Windows.Input;
using VaultCore;

namespace MauiPasswordVault.ViewModel;

public class SearchViewModel
{
    private readonly NavigationService navigationService;
    private readonly ErrorService errorService;
    private readonly MyVault vault;
    private String url = null!;

    public SearchViewModel(NavigationService navigationService, ErrorService errorService, MyVault vault)
    {
        this.navigationService = navigationService;
        this.errorService = errorService;
        this.vault = vault;

        UpdateCommand = new Command(
            execute: () =>
            {
                try
                {
                    Progress = 0.5;
                    InProgress = true;
                    this.vault.Download(url);
                }
                catch (Exception exception)
                {
                    this.errorService.LastErrorMessage = exception.Message;
                    this.errorService.LastErrorFull = exception.ToString();
                    this.errorService.CriticalError = true;
                    this.navigationService.NavigateToPage<ErrorPage>();
                }
                finally
                {
                    Progress = 0.0;
                    InProgress = false;
                }
            },
            canExecute: () =>
            {
                try
                {
                    return (!String.IsNullOrEmpty(url));
                }
                catch (Exception exception)
                {
                    this.errorService.LastErrorMessage = exception.Message;
                    this.errorService.LastErrorFull = exception.ToString();
                    this.errorService.CriticalError = true;
                    this.navigationService.NavigateToPage<ErrorPage>();
                }

                return false;
            });
    }

    public String Url
    {
        get { return url; }
        set
        {
            url = value;
            (UpdateCommand as Command)?.ChangeCanExecute();
        }
    }

    public bool InProgress { get; set; } = false;
    public double Progress { get; set; } = 0.0;

    public ICommand UpdateCommand { private set; get; }

    public MyVault Vault { get => vault; }

    public bool IsInitialized() => vault.IsInitialized();
    public bool IsUnlock() => vault.IsUnlock();
    public void Load()
    {
        vault.Load();
    }
}
