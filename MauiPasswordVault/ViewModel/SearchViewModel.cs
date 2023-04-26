using MauiPasswordVault.Service;
using MauiPasswordVault.View;
using System.ComponentModel;
using System.Windows.Input;
using VaultCore;
using static System.Net.Mime.MediaTypeNames;

namespace MauiPasswordVault.ViewModel;

public class SearchViewModel : INotifyPropertyChanged
{
    #region Private data
    private readonly NavigationService navigationService;
    private readonly ErrorService errorService;
    private readonly MyVault vault;
    private String url = null!;
    #endregion

    #region Properties
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
    public bool IsRefreshing { get; set; } = false;

    public ICommand UpdateCommand { private set; get; }
    public ICommand RefreshCommand { private set; get; }

    public MyVault Vault { get => vault; }

    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion    

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

        RefreshCommand = new Command(
            execute: () =>
            {
                try
                {
                    IsRefreshing = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRefreshing)));
                    vault.Load();
                    IsRefreshing = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRefreshing)));
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
                    IsRefreshing = false;
                }
            });
    }

    public bool IsInitialized() => vault.IsInitialized();
    public bool IsUnlock() => vault.IsUnlock();    
}
