using CommunityToolkit.Mvvm.ComponentModel;
using VaultGui.Resources.Strings;
using VaultGui.Services;
using Microsoft.Extensions.Logging;

namespace VaultGui.ViewModels;

public abstract partial class ViewModelBase(IDialogService dialogs, ILogger logger) : ObservableObject
{
    protected IDialogService Dialogs { get; } = dialogs;

    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    /// <summary>
    /// Run an action with the busy indicator on; unexpected errors are logged and shown.
    /// </summary>
    protected async Task RunAsync(Func<Task> action, string operation)
    {
        try
        {
            IsBusy = true;
            await action();
        }
        catch (Exception exception)
        {
            // Never log secrets: only the exception (messages contain no vault data).
            logger.LogError(exception, "{Operation} failed", operation);
            await Dialogs.AlertAsync(exception.Message, Loc.Get(Loc.Error));
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected static Task GoToAsync(string route) => Shell.Current.GoToAsync(route);

    protected static Task GoToAsync(string route, IDictionary<string, object> parameters) => Shell.Current.GoToAsync(route, parameters);
}
