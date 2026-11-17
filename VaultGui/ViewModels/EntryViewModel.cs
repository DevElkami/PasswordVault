using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VaultGui.Resources.Strings;
using VaultGui.Services;
using Microsoft.Extensions.Logging;
using VaultCore;
using VaultCore.Models;

namespace VaultGui.ViewModels;

/// <summary>
/// Add or edit one entry. Changes are saved to the vault file immediately.
/// </summary>
public partial class EntryViewModel(IVaultService vault, IDialogService dialogs, ILogger<EntryViewModel> logger)
    : ViewModelBase(dialogs, logger), IQueryAttributable
{
    public const string EntryParameter = "Entry";

    private MyPassword? original;

    [ObservableProperty]
    public partial string Title { get; set; } = Loc.Get(Loc.NewEntry);

    [ObservableProperty]
    public partial bool IsExisting { get; set; }

    [ObservableProperty]
    public partial string Site { get; set; } = "";

    [ObservableProperty]
    public partial string UserName { get; set; } = "";

    [ObservableProperty]
    public partial string Password { get; set; } = "";

    [ObservableProperty]
    public partial string Keyword { get; set; } = "";

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(EntryParameter, out object? value) && value is MyPassword entry)
        {
            original = entry;
            IsExisting = true;
            Title = Loc.Get(Loc.EditEntry);
            Site = entry.Data;
            UserName = entry.UserName;
            Password = entry.Password;
            Keyword = entry.Keyword;
        }
    }

    [RelayCommand]
    private Task SaveAsync() => RunAsync(async () =>
    {
        if (UserName.Trim().Length == 0 && Password.Length == 0)
        {
            await Dialogs.AlertAsync(Loc.Get(Loc.EntryEmpty));
            return;
        }

        MyPassword edited = new()
        {
            Data = Site.Trim(),
            UserName = UserName.Trim(),
            Password = Password,
            Keyword = Keyword.Trim(),
        };

        bool saved = original is null ? vault.Add(edited) : vault.Update(original, edited);
        if (!saved)
        {
            await Dialogs.AlertAsync(Loc.Get(Loc.Duplicate));
            return;
        }

        await vault.SaveAsync();
        await GoToAsync("..");
    }, nameof(SaveAsync));

    /// <summary>Back to the list without saving.</summary>
    [RelayCommand]
    private Task CancelAsync() => GoToAsync("..");

    [RelayCommand]
    private Task DeleteAsync() => RunAsync(async () =>
    {
        if (original is null || !await Dialogs.ConfirmAsync(Loc.Get(Loc.ConfirmDelete)))
            return;

        vault.Remove(original);
        await vault.SaveAsync();
        await GoToAsync("..");
    }, nameof(DeleteAsync));
}
