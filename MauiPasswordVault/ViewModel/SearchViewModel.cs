using CommunityToolkit.Mvvm.Input;
using MauiPasswordVault.Service;
using MauiPasswordVault.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VaultCore;
using VaultCore.Models;

namespace MauiPasswordVault.ViewModel;

public partial class SearchViewModel : INotifyPropertyChanged
{
    public SearchViewModel(NavigationService navigationService, ErrorService errorService, MyVault vault)
    {
        this.navigationService = navigationService;
        this.errorService = errorService;
        this.vault = vault;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    #region Private data
    private const String URL = "UPDATE_URL";
    private readonly object vaultLock = new object();
    private readonly NavigationService navigationService;
    private readonly ErrorService errorService;
    private readonly MyVault vault;
    private String url = Preferences.Default.Get(SearchViewModel.URL, "");
    private bool inProgress = false;
    private bool isLoading = false;
    private String searchEntry = "";
    private ObservableCollection<MyPassword> passwords = new ObservableCollection<MyPassword>();
    #endregion

    #region Command
    [RelayCommand(FlowExceptionsToTaskScheduler = true, CanExecute = nameof(CanUpdate))]
    public async Task UpdateAsync()
    {
        try
        {
            InProgress = true;
            Preferences.Default.Set(URL, url);
            await Task.Factory.StartNew(() =>
            {
                lock (vaultLock)
                {
                    vault.Download(url);
                }
            });
        }
        catch (Exception exception)
        {
            this.errorService.LastErrorMessage = exception.Message;
            this.errorService.LastErrorFull = exception.ToString();
            this.errorService.CriticalError = false;
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
            this.navigationService.NavigateToPage<ErrorPage>();
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
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
            await Task.Factory.StartNew(() =>
            {
                lock (vaultLock)
                {
                    vault.Load();
                }
            });
        }
        catch (Exception exception)
        {
            this.errorService.LastErrorMessage = exception.Message;
            this.errorService.LastErrorFull = exception.ToString();
            this.errorService.CriticalError = true;
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
            this.navigationService.NavigateToPage<ErrorPage>();
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand(FlowExceptionsToTaskScheduler = true, CanExecute = nameof(CanSearch))]
    public async Task SearchAsync()
    {
        try
        {
            IsLoading = true;
            await Task.Factory.StartNew(() =>
            {
                ObservableCollection<MyPassword> results = new ObservableCollection<MyPassword>();

                lock (vaultLock)
                {
                    foreach (MyPassword myPassword in vault)
                    {
                        if (myPassword.ToStr().Contains(searchEntry))
                            results.Add(myPassword);
                    }
                }

                Passwords = results;
            });
        }
        catch (Exception exception)
        {
            this.errorService.LastErrorMessage = exception.Message;
            this.errorService.LastErrorFull = exception.ToString();
            this.errorService.CriticalError = false;
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
            this.navigationService.NavigateToPage<ErrorPage>();
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
        }
        finally
        {
            IsLoading = false;
        }
    }

    private bool CanSearch()
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Url)));
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

    public String SearchEntry
    {
        get => searchEntry;
        set
        {
            searchEntry = value;
            (SearchCommand as Command)?.ChangeCanExecute();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchEntry)));
        }
    }

    public ObservableCollection<MyPassword> Passwords
    {
        get => passwords;
        set
        {
            passwords = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Passwords)));
        }
    }
    #endregion

    public bool IsInitialized() => vault.IsInitialized();
    public bool IsUnlock() => vault.IsUnlock();
}
