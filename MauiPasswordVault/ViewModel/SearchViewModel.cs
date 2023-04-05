using VaultCore;

namespace MauiPasswordVault.ViewModel;

public class SearchViewModel
{
    private readonly MyVault vault;
    public SearchViewModel(MyVault vault)
    {
        this.vault = vault;
    }

    public bool IsInitialized() => vault.IsInitialized();
    public bool IsUnlock() => vault.IsUnlock();
}
