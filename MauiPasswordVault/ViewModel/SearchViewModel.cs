using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultCore;

namespace MauiPasswordVault.ViewModel
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private readonly MyVault vault;
        public SearchViewModel(MyVault vault)
        {
            this.vault = vault;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
