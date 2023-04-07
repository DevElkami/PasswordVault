using VaultCore;

namespace MauiPasswordVault.ViewModel;

public class SearchViewModel
{
    private readonly MyVault vault;
    public SearchViewModel(MyVault vault)
    {
        this.vault = vault;
    }

    public MyVault Vault { get => vault; }

    public bool IsInitialized() => vault.IsInitialized();
    public bool IsUnlock() => vault.IsUnlock();
    public void Load()
    {
        vault.Load();
#if DEBUG
        if(vault.Count == 0)
            vault.Add(new VaultCore.Models.MyPassword());
#endif
    }
}
