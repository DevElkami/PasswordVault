using VaultCore.Crypto;
using VaultCore.Models;

namespace VaultCore.Merge;

/// <summary>
/// Merges two decrypted versions of the same vault: the one in memory and the one in the file,
/// when the file was changed by another instance of the app since it was read.
/// <para>
/// Pure and deterministic: both sides get the same result from the same two versions.
/// </para>
/// Rules:
/// <list type="bullet">
/// <item>Same id: the newest version wins.</item>
/// <item>A deletion wins over versions older than it, never over a later edit.</item>
/// <item>Same content under two ids (added on both sides): one is kept, the other recorded as deleted.</item>
/// <item>Deletion records older than <see cref="TombstoneLifetime"/> are dropped.</item>
/// </list>
/// </summary>
internal static class VaultMerge
{
    public static readonly TimeSpan TombstoneLifetime = TimeSpan.FromDays(365);

    /// <summary>Clean up one version: ids for old entries, duplicates removed, deletions applied.</summary>
    public static VaultPayload Normalize(VaultPayload payload, DateTimeOffset now) => Merge(payload, new VaultPayload(), now);

    public static VaultPayload Merge(VaultPayload local, VaultPayload other, DateTimeOffset now)
    {
        Dictionary<string, MyPassword> entries = new(StringComparer.Ordinal);
        List<string> order = [];
        foreach (MyPassword entry in local.Entries.Concat(other.Entries))
        {
            if (string.IsNullOrEmpty(entry.Id))
                entry.Id = MyPassword.ContentId(entry);

            if (!entries.TryGetValue(entry.Id, out MyPassword? current))
            {
                entries.Add(entry.Id, entry);
                order.Add(entry.Id);
            }
            else if (IsNewer(entry, current))
            {
                entries[entry.Id] = entry;
            }
        }

        Dictionary<string, DateTimeOffset> deleted = new(StringComparer.Ordinal);
        foreach (VaultTombstone tombstone in local.Deleted.Concat(other.Deleted))
            Tombstone(deleted, tombstone.Id, tombstone.At);

        foreach ((string id, DateTimeOffset at) in deleted.ToList())
        {
            if (!entries.TryGetValue(id, out MyPassword? entry))
                continue;
            if (at > entry.Modified)
                entries.Remove(id);
            else
                deleted.Remove(id); // Edited after the deletion: the edit wins.
        }

        foreach (IGrouping<string, MyPassword> group in entries.Values.GroupBy(x => x.ContentKey(), StringComparer.Ordinal).ToList())
        {
            if (!group.Skip(1).Any())
                continue;

            MyPassword keep = group.OrderByDescending(x => x.Modified).ThenBy(x => x.Id, StringComparer.Ordinal).First();
            foreach (MyPassword duplicate in group.Where(x => !ReferenceEquals(x, keep)))
            {
                entries.Remove(duplicate.Id);
                Tombstone(deleted, duplicate.Id, duplicate.Modified);
            }
        }

        DateTimeOffset expiry = now - TombstoneLifetime;
        return new VaultPayload
        {
            Entries = [.. order.Where(entries.ContainsKey).Select(id => entries[id])],
            Deleted =
            [
                .. deleted
                    .Where(x => x.Value >= expiry && !entries.ContainsKey(x.Key))
                    .OrderBy(x => x.Key, StringComparer.Ordinal)
                    .Select(x => new VaultTombstone { Id = x.Key, At = x.Value }),
            ],
        };
    }

    private static bool IsNewer(MyPassword candidate, MyPassword current) =>
        candidate.Modified != current.Modified
            ? candidate.Modified > current.Modified
            : string.CompareOrdinal(candidate.ToStr(), current.ToStr()) > 0;

    private static void Tombstone(Dictionary<string, DateTimeOffset> deleted, string id, DateTimeOffset at)
    {
        if (!deleted.TryGetValue(id, out DateTimeOffset existing) || at > existing)
            deleted[id] = at;
    }
}
