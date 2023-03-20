using System.ComponentModel;
using VaultCore;

namespace MauiPasswordVault.ViewModel;

public class InitViewModel : INotifyPropertyChanged
{
    private readonly MyVault vault;
    public InitViewModel(MyVault vault)
    {
        this.vault = vault;
    }
    public event PropertyChangedEventHandler PropertyChanged;
}
