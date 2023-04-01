using System.ComponentModel;
using VaultCore;

namespace MauiPasswordVault.ViewModel;

public class CheckViewModel : INotifyPropertyChanged
{
    private readonly MyVault vault;
    public CheckViewModel(MyVault vault)
    {
        this.vault = vault;
    }

    public bool IsInitialized() => vault.IsInitialized();        

    public event PropertyChangedEventHandler PropertyChanged;
}
