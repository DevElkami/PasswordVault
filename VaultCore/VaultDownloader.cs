namespace VaultCore;

/// <summary>
/// Downloads a vault file published on a web server (the 3.x "update from URL" feature).
/// </summary>
public static class VaultDownloader
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(30);

    /// <summary>Largest accepted file: far above any real vault, stops a wrong URL from filling memory.</summary>
    public const int MaxSize = 50 * 1024 * 1024;

    /// <exception cref="ArgumentException">Not an https:// URL.</exception>
    public static async Task<byte[]> DownloadAsync(string url, CancellationToken cancellationToken = default)
    {
        if (!Uri.TryCreate(url?.Trim(), UriKind.Absolute, out Uri? uri) || uri.Scheme != Uri.UriSchemeHttps)
            throw new ArgumentException("Only https:// URLs are allowed.", nameof(url));

        using HttpClient client = new() { Timeout = Timeout, MaxResponseContentBufferSize = MaxSize };
        return await client.GetByteArrayAsync(uri, cancellationToken).ConfigureAwait(false);
    }
}
