using VaultGui.Resources.Strings;

namespace VaultGui.Services;

/// <summary>
/// Alerts and confirmations, so view models do not depend on pages.
/// </summary>
public interface IDialogService
{
    Task AlertAsync(string message, string? title = null);
    Task<bool> ConfirmAsync(string message, string? title = null);
}

public sealed class DialogService : IDialogService
{
    private static Page? CurrentPage => Shell.Current?.CurrentPage ?? Application.Current?.Windows.FirstOrDefault()?.Page;

    public Task AlertAsync(string message, string? title = null) =>
        MainThread.InvokeOnMainThreadAsync(() =>
            CurrentPage?.DisplayAlertAsync(title ?? Loc.Get(Loc.AppTitle), message, Loc.Get(Loc.Ok)) ?? Task.CompletedTask);

    public Task<bool> ConfirmAsync(string message, string? title = null) =>
        MainThread.InvokeOnMainThreadAsync(() =>
            CurrentPage?.DisplayAlertAsync(title ?? Loc.Get(Loc.AppTitle), message, Loc.Get(Loc.Yes), Loc.Get(Loc.No)) ?? Task.FromResult(false));
}
