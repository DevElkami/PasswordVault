using VaultCore.Models;

namespace VaultCore;

/// <summary>
/// Finds entries that are duplicates or look alike: same site once the address is simplified
/// ("https://www.Example.com/login" and "example.com" are the same site).
/// </summary>
public static class VaultDuplicates
{
    /// <summary>
    /// Groups of two or more entries for the same site, ordered by site; inside a group, by login.
    /// Entries with neither site nor keyword are ignored.
    /// </summary>
    public static IReadOnlyList<IReadOnlyList<MyPassword>> Find(IEnumerable<MyPassword> entries) =>
        [.. entries
            .Select(entry => (Key: SiteKey(entry), Entry: entry))
            .Where(x => x.Key.Length > 0)
            .GroupBy(x => x.Key, StringComparer.Ordinal)
            .Where(group => group.Count() > 1)
            .OrderBy(group => group.Key, StringComparer.Ordinal)
            .Select(group => (IReadOnlyList<MyPassword>)[.. group
                .Select(x => x.Entry)
                .OrderBy(entry => entry.UserName, StringComparer.OrdinalIgnoreCase)])];

    /// <summary>
    /// Simplified site used to compare entries: lower case, without scheme, "www.", port, path or query.
    /// The keyword is used when there is no site. Free text ("My bank / joint account") is only trimmed and lowered.
    /// </summary>
    public static string SiteKey(MyPassword entry)
    {
        string site = (string.IsNullOrWhiteSpace(entry.Data) ? entry.Keyword : entry.Data).Trim().ToLowerInvariant();
        if (site.Any(char.IsWhiteSpace))
            return site;

        int scheme = site.IndexOf("://", StringComparison.Ordinal);
        if (scheme >= 0)
            site = site[(scheme + 3)..];

        int end = site.IndexOfAny(['/', '?', '#']);
        if (end >= 0)
            site = site[..end];

        int port = site.LastIndexOf(':');
        if (port > 0 && port < site.Length - 1 && site.AsSpan(port + 1).IndexOfAnyExceptInRange('0', '9') < 0)
            site = site[..port];

        if (site.StartsWith("www.", StringComparison.Ordinal))
            site = site[4..];

        return site;
    }
}
