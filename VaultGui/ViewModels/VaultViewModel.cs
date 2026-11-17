using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VaultGui.Resources.Strings;
using VaultGui.Services;
using Microsoft.Extensions.Logging;
using VaultCore;
using VaultCore.Models;

namespace VaultGui.ViewModels;

/// <summary>
/// Main screen: search, copy, open entries.
/// </summary>
public partial class VaultViewModel(IVaultService vault, VaultUpdateService update, UploadService upload, ISecureClipboard clipboard, IDialogService dialogs, ILogger<VaultViewModel> logger)
    : ViewModelBase(dialogs, logger)
{
    private CancellationTokenSource? statusTimer;

    public ObservableCollection<MyPassword> Entries { get; } = [];

    [ObservableProperty]
    public partial string SearchText { get; set; } = "";

    [ObservableProperty]
    public partial string EmptyMessage { get; set; } = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasStatus))]
    public partial string StatusMessage { get; set; } = "";

    public bool HasStatus => StatusMessage.Length > 0;

    #region Update from the remembered https address
    public bool IsUpdateConfigured => update.IsConfigured;

    [RelayCommand]
    private Task UpdateAsync() => RunAsync(async () =>
    {
        UpdateOutcome outcome = await update.UpdateAsync();
        Refresh();
        UpdateUploadPending();

        if (VaultUpdateService.Message(outcome) is not string message)
            return;
        if (outcome is UpdateOutcome.Updated or UpdateOutcome.UpToDate)
            await ShowStatusAsync(message);
        else
            await Dialogs.AlertAsync(message);
    }, nameof(UpdateAsync));
    #endregion

    #region Send to server
    public bool IsUploadConfigured => upload.IsConfigured;

    /// <summary>"Changes not sent yet" reminder (sending is manual). Empty when everything was sent.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasUploadPending))]
    public partial string UploadPending { get; set; } = "";

    public bool HasUploadPending => UploadPending.Length > 0;

    private void OnUploadStateChanged(object? sender, EventArgs e) => MainThread.BeginInvokeOnMainThread(UpdateUploadPending);

    private void UpdateUploadPending() => UploadPending = upload.HasUnsentChanges ? Loc.Get(Loc.UploadPending) : "";

    [RelayCommand]
    private Task UploadAsync() => RunAsync(async () =>
    {
        if (await upload.SendAsync())
            await ShowStatusAsync(Loc.Get(Loc.UploadDone));
    }, nameof(UploadAsync));
    #endregion

    public void OnAppearing()
    {
        upload.StateChanged += OnUploadStateChanged;
        UpdateUploadPending();
        Refresh();
    }

    public void OnDisappearing()
    {
        upload.StateChanged -= OnUploadStateChanged;
    }

    partial void OnSearchTextChanged(string value) => Refresh();

    public void Refresh()
    {
        Entries.Clear();
        if (!vault.IsUnlocked)
            return;

        IEnumerable<MyPassword> found = vault.Search(SearchText);
        if (ShowDuplicates)
        {
            // Same site side by side, so the extra ones are easy to compare and delete.
            HashSet<MyPassword> similar = new(VaultDuplicates.Find(vault.Entries).SelectMany(group => group), ReferenceEqualityComparer.Instance);
            found = found.Where(similar.Contains).OrderBy(VaultDuplicates.SiteKey, StringComparer.Ordinal).ThenBy(e => e.UserName, StringComparer.CurrentCultureIgnoreCase);
        }
        else
        {
            found = found.OrderBy(e => e.Title, StringComparer.CurrentCultureIgnoreCase);
        }

        foreach (MyPassword entry in found)
            Entries.Add(entry);

        EmptyMessage = Loc.Get(ShowDuplicates ? Loc.NoDuplicates : string.IsNullOrWhiteSpace(SearchText) ? Loc.NoEntries : Loc.NoResults);
    }

    #region Duplicates
    /// <summary>Only entries that share their site with another one (see <see cref="VaultDuplicates"/>).</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DuplicatesText))]
    public partial bool ShowDuplicates { get; set; }

    public string DuplicatesText => Loc.Get(ShowDuplicates ? Loc.ShowAll : Loc.Duplicates);

    [RelayCommand]
    private async Task ToggleDuplicatesAsync()
    {
        ShowDuplicates = !ShowDuplicates;
        Refresh();
        if (ShowDuplicates && Entries.Count > 0)
            await ShowStatusAsync(Loc.Format(Loc.DuplicatesFound, Entries.Count));
    }
    #endregion

    [RelayCommand]
    private Task DeleteAsync(MyPassword entry) => RunAsync(async () =>
    {
        if (!await Dialogs.ConfirmAsync(Loc.Format(Loc.ConfirmDeleteEntry, entry.Title, entry.UserName)))
            return;

        vault.Remove(entry);
        await vault.SaveAsync();
        Refresh();
    }, nameof(DeleteAsync));

    [RelayCommand]
    private Task AddAsync() => GoToAsync("entry");

    [RelayCommand]
    private Task EditAsync(MyPassword entry) =>
        GoToAsync("entry", new Dictionary<string, object> { [EntryViewModel.EntryParameter] = entry });

    [RelayCommand]
    private Task CopyUserAsync(MyPassword entry) => CopyAsync(entry.UserName);

    [RelayCommand]
    private Task CopyPasswordAsync(MyPassword entry) => CopyAsync(entry.Password);

    [RelayCommand]
    private Task SettingsAsync() => GoToAsync("settings");

    [RelayCommand]
    private void Lock()
    {
        SearchText = "";
        ShowDuplicates = false;
        vault.Lock();
    }

    private async Task CopyAsync(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        await clipboard.CopyAsync(text);
        await ShowStatusAsync(Loc.Get(Loc.Copied));
    }

    private async Task ShowStatusAsync(string message)
    {
        statusTimer?.Cancel();
        statusTimer = new CancellationTokenSource();
        StatusMessage = message;
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(3), statusTimer.Token);
            StatusMessage = "";
        }
        catch (TaskCanceledException)
        {
        }
    }
}
