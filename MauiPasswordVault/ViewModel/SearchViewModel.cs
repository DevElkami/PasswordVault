using CommunityToolkit.Mvvm.Input;
using MauiPasswordVault.Service;
using MauiPasswordVault.View;
using System.ComponentModel;
using System.Windows.Input;
using VaultCore;

namespace MauiPasswordVault.ViewModel;

public partial class SearchViewModel : INotifyPropertyChanged
{
    #region Private data
    private const String URL = "UPDATE_URL";
    private readonly NavigationService navigationService;
    private readonly ErrorService errorService;
    private readonly MyVault vault;
    private String url = Preferences.Default.Get(SearchViewModel.URL, "");
    private bool inProgress = false;
    private bool isLoading = false;
    #endregion

    #region Command
    [RelayCommand(FlowExceptionsToTaskScheduler = true, CanExecute = nameof(CanUpdate))]
    public async Task UpdateAsync()
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

    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    public async Task LoadAsync()
    {
        try
        {
            IsLoading = true;
            await Task.Factory.StartNew(() => vault.Load());
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
            IsLoading = false;
        }
    }
    #endregion

    #region Properties
    public String Url
    {
        get { return url; }
        set
        {
            url = value;
            (SearchCommand as Command)?.ChangeCanExecute();
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

    public bool IsLoading
    {
        get => isLoading;
        set
        {
            isLoading = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
        }
    }

    public String SearchEntry { get; set; } = null!;

    public ICommand SearchCommand { private set; get; }

    public MyVault Vault { get => vault; }

    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion    

    public SearchViewModel(NavigationService navigationService, ErrorService errorService, MyVault vault)
    {
        this.navigationService = navigationService;
        this.errorService = errorService;
        this.vault = vault;

        SearchCommand = new Command(
            execute: () =>
            {
                try
                {
                    InProgress = true;
                    //...
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
