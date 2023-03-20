using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
