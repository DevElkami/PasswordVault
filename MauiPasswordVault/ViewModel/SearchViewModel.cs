using CommunityToolkit.Mvvm.Input;
using MauiPasswordVault.Service;
using MauiPasswordVault.View;
using System.ComponentModel;
using System.Windows.Input;
using VaultCore;

namespace MauiPasswordVault.ViewModel;

public partial class SearchViewModel : INotifyPropertyChanged
{
    public const String URL = "UPDATE_URL";
    #region Private data
    private readonly NavigationService navigationService;
    private readonly ErrorService errorService;
    private readonly MyVault vault;
    private String url = Preferences.Default.Get(SearchViewModel.URL, "");
    private bool inProgress = false;
    #endregion

    #region Command
    [RelayCommand(FlowExceptionsToTaskScheduler = true, CanExecute = nameof(CanUpdate))]
    private async Task UpdateAsync(CancellationToken token)
    {
        try
        {
            InProgress = true;
            Preferences.Default.Set(URL, url);
            await Task.Factory.StartNew(() => vault.Download(url));
        }
        catch (Exception exception)
        {
            this.errorService.LastErrorMessage = exception.Message;
            this.errorService.LastErrorFull = exception.ToString();
            this.errorService.CriticalError = false;
            this.navigationService.NavigateToPage<ErrorPage>();
        }
        finally
        {
            InProgress = false;
        }
    }

    private bool CanUpdate()
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
    }
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

    public bool InProgress
    {
        get => inProgress;
        set
        {
            inProgress = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InProgress)));
        }
    }

    public bool IsRefreshing { get; set; } = false;
    public String SearchEntry { get; set; } = null!;

    public ICommand RefreshCommand { private set; get; }
    public ICommand SearchCommand { private set; get; }

    public MyVault Vault { get => vault; }

    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion    

    public SearchViewModel(NavigationService navigationService, ErrorService errorService, MyVault vault)
    {
        this.navigationService = navigationService;
        this.errorService = errorService;
        this.vault = vault;

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

        SearchCommand = new Command(
            execute: () =>
            {
                try
                {
                    InProgress = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InProgress)));
                    Thread.Sleep(2000);
                    vault.Load();
                    InProgress = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InProgress)));
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
                    return (!String.IsNullOrEmpty(SearchEntry) && (SearchEntry.Length >= 3));
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

    public bool IsInitialized() => vault.IsInitialized();
    public bool IsUnlock() => vault.IsUnlock();
}
