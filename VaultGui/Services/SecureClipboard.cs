namespace VaultGui.Services;

/// <summary>
/// Copies secrets to the clipboard and clears them after a delay.
/// <list type="bullet">
/// <item>Android 13+: the clip is flagged sensitive (hidden from the clipboard preview).</item>
/// <item>Windows: kept out of clipboard history (Win+V) and cloud clipboard.</item>
/// </list>
/// </summary>
public interface ISecureClipboard
{
    Task CopyAsync(string text);
}

public sealed partial class SecureClipboard : ISecureClipboard
{
    public static readonly TimeSpan ClearDelay = TimeSpan.FromSeconds(30);

    private int generation;

    public async Task CopyAsync(string text)
    {
        int current = Interlocked.Increment(ref generation);
        await MainThread.InvokeOnMainThreadAsync(() => SetText(text));

        _ = ClearLaterAsync(text, current);
    }

    private async Task ClearLaterAsync(string text, int copyGeneration)
    {
        await Task.Delay(ClearDelay);

        // A newer copy will clear itself.
        if (copyGeneration != Volatile.Read(ref generation))
            return;

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            // Only clear what we put there. If the clipboard cannot be read (Android app in the
            // background), clear anyway: leaving a password behind is worse.
            string? currentText = await TryGetTextAsync();
            if (currentText is null || currentText == text)
                Clear();
        });
    }

    private static async Task<string?> TryGetTextAsync()
    {
        try
        {
            return await Clipboard.Default.GetTextAsync();
        }
        catch (Exception)
        {
            return null;
        }
    }

#if ANDROID
    private static Android.Content.ClipboardManager? Manager =>
        Platform.AppContext.GetSystemService(Android.Content.Context.ClipboardService) as Android.Content.ClipboardManager;

    private static void SetText(string text)
    {
        if (Manager is not { } manager)
            return;

        if (Android.Content.ClipData.NewPlainText("", text) is not { } clip)
            return;

        if (OperatingSystem.IsAndroidVersionAtLeast(24) && clip.Description is { } description)
        {
            Android.OS.PersistableBundle extras = new();
            // ClipDescription.EXTRA_IS_SENSITIVE (API 33), harmless on older versions.
            extras.PutBoolean("android.content.extra.IS_SENSITIVE", true);
            description.Extras = extras;
        }
        manager.PrimaryClip = clip;
    }

    private static void Clear()
    {
        if (Manager is not { } manager)
            return;

        if (OperatingSystem.IsAndroidVersionAtLeast(28))
            manager.ClearPrimaryClip();
        else
            manager.PrimaryClip = Android.Content.ClipData.NewPlainText("", "");
    }
#elif WINDOWS
    private static void SetText(string text)
    {
        Windows.ApplicationModel.DataTransfer.DataPackage package = new();
        package.SetText(text);
        Windows.ApplicationModel.DataTransfer.Clipboard.SetContentWithOptions(package, new Windows.ApplicationModel.DataTransfer.ClipboardContentOptions
        {
            IsAllowedInHistory = false,
            IsRoamable = false,
        });
    }

    private static void Clear() => Windows.ApplicationModel.DataTransfer.Clipboard.Clear();
#else
    private static void SetText(string text) => Clipboard.Default.SetTextAsync(text);

    private static void Clear() => Clipboard.Default.SetTextAsync(null);
#endif
}
