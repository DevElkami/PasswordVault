using MauiPasswordVault.Service;
using System.ComponentModel;
using System.Windows.Input;
using VaultCore;

namespace MauiPasswordVault.ViewModel;

public class InitViewModel : INotifyPropertyChanged
{
    private readonly NavigationService navigationService;
    private readonly MyVault vault;
    public InitViewModel(NavigationService navigationService, MyVault vault)
    {
        this.navigationService = navigationService;
        this.vault = vault;

        InitCommand = new Command(
            execute: () =>
            {
                /*PersonEdit = new PersonViewModel();
                PersonEdit.PropertyChanged += OnPersonEditPropertyChanged;
                IsEditing = true;
                RefreshCanExecutes();*/
            },
            canExecute: () =>
            {
                return true;// !IsEditing;
            });
    }
    public event PropertyChangedEventHandler PropertyChanged = null!;

    public ICommand InitCommand { private set; get; }    
}
