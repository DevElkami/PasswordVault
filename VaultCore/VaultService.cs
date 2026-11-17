using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using VaultCore.Crypto;
using VaultCore.Models;
using VaultCore.Storage;
using VaultCore.Merge;

namespace VaultCore;

/// <summary>
/// UI-independent vault: unlock, edit, save, merge. Thread-safe.
/// <para>
/// Files use format v2 (see <see cref="VaultCipher"/>); a v1 vault is migrated
/// automatically the first time it is unlocked.
/// </para>
/// </summary>
public interface IVaultService
{
    /// <summary>A vault exists (v2, or v1 waiting for migration).</summary>
    bool IsInitialized { get; }

    /// <summary>Password verified, key in memory, entries loaded.</summary>
    bool IsUnlocked { get; }

    /// <summary>Only a v1 vault exists: it will be migrated on the next successful unlock.</summary>
    bool NeedsMigration { get; }

    /// <summary>Snapshot of the decrypted entries (empty while locked).</summary>
    IReadOnlyList<MyPassword> Entries { get; }

    /// <summary>Raised after Lock() (manual, auto-lock, or vault replaced).</summary>
    event EventHandler? Locked;

    /// <summary>Raised after the local vault file was written.</summary>
    event EventHandler? Saved;

    /// <summary>Raised when the vault key changes (master password changed).</summary>
    event EventHandler? KeyChanged;

    /// <summary>
    /// Identifies the key of the vault file (its KDF salt), readable without unlocking.
    /// Changes whenever the master password changes. Null if there is no v2 file.
    /// </summary>
    string? FileKeyId { get; }

    Task CreateAsync(string password, CancellationToken cancellationToken = default);
    Task<bool> UnlockAsync(string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unlock with a key previously obtained from <see cref="ExportKey"/> (quick unlock).
    /// False if the key does not open the current vault file (for example after a password change).
    /// </summary>
    Task<bool> UnlockWithKeyAsync(byte[] key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Copy of the derived vault key, to be stored protected by the platform (biometrics).
    /// The caller must zero it after use.
    /// </summary>
    byte[] ExportKey();
    void Lock();
    Task ChangePasswordAsync(string newPassword, CancellationToken cancellationToken = default);
    void Erase();

    /// <summary>Add an entry. Returns false if an entry with the same content already exists.</summary>
    bool Add(MyPassword entry);

    /// <summary>
    /// Replace <paramref name="original"/> by <paramref name="edited"/> (keeps its id).
    /// Returns false if another entry already has the edited content.
    /// </summary>
    bool Update(MyPassword original, MyPassword edited);

    void Remove(MyPassword entry);
    void ReplaceAll(IEnumerable<MyPassword> entries);

    /// <summary>Entries matching <paramref name="text"/> in any field (case-insensitive). "*" or empty returns all.</summary>
    IReadOnlyList<MyPassword> Search(string? text);

    /// <summary>
    /// Write the entries to the vault file. If the file was changed by someone else
    /// since it was last read (another instance of the app), both versions are merged first.
    /// </summary>
    Task SaveAsync(CancellationToken cancellationToken = default);

    /// <summary>Merge changes made to the vault file by someone else into the entries in memory.</summary>
    Task ReloadAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Replace the local vault with a copy downloaded over HTTPS.
    /// Returns true if still unlocked (same vault), false if it uses another password and is now locked.
    /// </summary>
    Task<bool> DownloadAsync(string url, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create the vault from a 3.x data file (for example downloaded from a web server), with its master password.
    /// The vault is unlocked afterwards, with the same password. False if the password is wrong.
    /// </summary>
    /// <exception cref="InvalidOperationException">A vault already exists.</exception>
    /// <exception cref="VaultFormatException">Not a 3.x data file.</exception>
    Task<bool> CreateFromLegacyDataAsync(byte[] content, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add the entries of a 3.x data file to the unlocked vault (entries already there are skipped), then save.
    /// Returns the number of entries added, or null if <paramref name="password"/> is not the one of that file.
    /// </summary>
    /// <exception cref="VaultFormatException">Not a 3.x data file.</exception>
    Task<int?> ImportLegacyDataAsync(byte[] content, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Replace the local vault with a complete v2 vault file (copied from another device).
    /// Returns true if still unlocked (same vault), false if it uses another password and is now locked.
    /// </summary>
    bool ImportVaultFile(byte[] content);

    /// <summary>Current content of the local vault file.</summary>
    byte[] ReadVaultFile();
}

public sealed class VaultService : IVaultService
{
    private readonly VaultOptions options;
    private readonly IVaultStorage storage;

    /// <summary>Guards the in-memory state below.</summary>
    private readonly object sync = new();

    /// <summary>Serializes file read-merge-write sequences.</summary>
    private readonly object fileSync = new();

    private readonly List<MyPassword> entries = [];
    private readonly List<VaultTombstone> deleted = [];

    /// <summary>Derived key, present only while unlocked.</summary>
    private byte[]? key;

    /// <summary>KDF parameters matching <see cref="key"/>.</summary>
    private VaultKdfParameters? kdf;

    /// <summary>SHA-256 of the vault file as last read or written, to detect changes made by someone else.</summary>
    private byte[]? lastFileHash;

    public VaultService() : this(VaultOptions.CreateDefault()) { }

    public VaultService(VaultOptions options) : this(options, new FileVaultStorage(options.VaultFilePath)) { }

    public VaultService(VaultOptions options, IVaultStorage storage)
    {
        this.options = options;
        this.storage = storage;
    }

    public event EventHandler? Locked;
    public event EventHandler? Saved;
    public event EventHandler? KeyChanged;

    private DateTimeOffset Now => options.TimeProvider.GetUtcNow();

    #region State
    public bool IsInitialized => storage.Exists() || LegacyVaultExists();

    public bool IsUnlocked
    {
        get { lock (sync) return key is not null; }
    }

    public bool NeedsMigration => !storage.Exists() && LegacyVaultExists();

    public IReadOnlyList<MyPassword> Entries
    {
        get { lock (sync) return [.. entries]; }
    }

    public string? FileKeyId
    {
        get
        {
            try
            {
                lock (fileSync)
                    return storage.Exists() ? Convert.ToBase64String(VaultCipher.Parse(storage.Read()).Kdf.Salt) : null;
            }
            catch (Exception exception) when (exception is VaultFormatException or IOException)
            {
                return null;
            }
        }
    }

    private bool LegacyVaultExists() =>
        options.LegacyKeyFilePath is not null && File.Exists(options.LegacyKeyFilePath);
    #endregion

    #region Lock / unlock
    public Task CreateAsync(string password, CancellationToken cancellationToken = default) =>
        Task.Run(() => Create(password), cancellationToken);

    /// <summary>
    /// Create a new empty vault. The vault is unlocked afterwards.
    /// </summary>
    /// <exception cref="InvalidOperationException">A vault already exists.</exception>
    public void Create(string password)
    {
        if (IsInitialized)
            throw new InvalidOperationException("A vault already exists. Erase it first.");

        Lock();
        VaultKdfParameters newKdf = VaultCipher.NewKdfParameters(options.Pbkdf2Iterations);
        byte[] newKey = VaultCipher.DeriveKey(password, newKdf);
        lock (fileSync)
            WriteVault(new VaultPayload(), newKey, newKdf);

        lock (sync)
        {
            key = newKey;
            kdf = newKdf;
        }
        Saved?.Invoke(this, EventArgs.Empty);
    }

    public Task<bool> UnlockAsync(string password, CancellationToken cancellationToken = default) =>
        Task.Run(() => Unlock(password), cancellationToken);

    /// <summary>
    /// Verify the master password, unlock and load the entries. Migrates a v1 vault to v2 on success.
    /// </summary>
    public bool Unlock(string password)
    {
        if (!IsInitialized)
            return false;

        Lock();

        if (!storage.Exists())
            return MigrateFromV1(password);

        lock (fileSync)
        {
            byte[] content = storage.Read();
            VaultEnvelope envelope = VaultCipher.Parse(content);
            return OpenWith(content, envelope, VaultCipher.DeriveKey(password, envelope.Kdf));
        }
    }

    public Task<bool> UnlockWithKeyAsync(byte[] key, CancellationToken cancellationToken = default) =>
        Task.Run(() => UnlockWithKey(key), cancellationToken);

    /// <inheritdoc cref="IVaultService.UnlockWithKeyAsync"/>
    public bool UnlockWithKey(byte[] candidateKey)
    {
        Lock();
        if (candidateKey.Length != VaultCipher.KeySize || !storage.Exists())
            return false;

        lock (fileSync)
        {
            byte[] content = storage.Read();
            return OpenWith(content, VaultCipher.Parse(content), (byte[])candidateKey.Clone());
        }
    }

    /// <summary>
    /// Try <paramref name="candidate"/> on the file; on success it becomes the session key (owned, not copied).
    /// Caller holds <see cref="fileSync"/>.
    /// </summary>
    private bool OpenWith(byte[] content, VaultEnvelope envelope, byte[] candidate)
    {
        if (!VaultCipher.TryOpen(envelope, candidate, out byte[] plaintext))
        {
            CryptographicOperations.ZeroMemory(candidate);
            return false;
        }

        VaultPayload loaded = VaultMerge.Normalize(DeserializePayload(plaintext), Now);
        lock (sync)
        {
            key = candidate;
            kdf = envelope.Kdf;
            SetData(loaded);
        }
        lastFileHash = SHA256.HashData(content);
        return true;
    }

    public byte[] ExportKey()
    {
        lock (sync)
        {
            if (key is null)
                throw new InvalidOperationException("The vault is locked.");
            return (byte[])key.Clone();
        }
    }

    /// <summary>
    /// Forget the key and the decrypted entries.
    /// </summary>
    public void Lock()
    {
        bool wasUnlocked;
        lock (sync)
        {
            wasUnlocked = key is not null;
            if (key is not null)
                CryptographicOperations.ZeroMemory(key);
            key = null;
            kdf = null;
            entries.Clear();
            deleted.Clear();
        }

        if (wasUnlocked)
            Locked?.Invoke(this, EventArgs.Empty);
    }

    public Task ChangePasswordAsync(string newPassword, CancellationToken cancellationToken = default) =>
        Task.Run(() => ChangePassword(newPassword), cancellationToken);

    /// <summary>
    /// Re-encrypt the vault with a new master password (new salt, current iteration setting).
    /// </summary>
    public void ChangePassword(string newPassword)
    {
        EnsureUnlocked();

        VaultKdfParameters newKdf = VaultCipher.NewKdfParameters(options.Pbkdf2Iterations);
        byte[] newKey = VaultCipher.DeriveKey(newPassword, newKdf);

        lock (fileSync)
        {
            // Pick up changes made to the file since it was read, while the old key still opens it.
            MergeExternalChanges();
            WriteVault(Snapshot(), newKey, newKdf);

            lock (sync)
            {
                if (key is not null)
                    CryptographicOperations.ZeroMemory(key);
                key = newKey;
                kdf = newKdf;
            }
        }
        Saved?.Invoke(this, EventArgs.Empty);
        KeyChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Delete the vault (v2 file and v1 key file). Cannot be undone.
    /// </summary>
    public void Erase()
    {
        Lock();
        lock (fileSync)
        {
            storage.Delete();
            lastFileHash = null;
            if (options.LegacyKeyFilePath is not null)
                File.Delete(options.LegacyKeyFilePath);
        }
    }
    #endregion

    #region Entries
    public bool Add(MyPassword entry)
    {
        lock (sync)
        {
            EnsureUnlocked();
            if (entries.Any(entry.HasSameContent))
                return false;

            if (string.IsNullOrEmpty(entry.Id) || entries.Any(x => x.Id == entry.Id))
                entry.Id = Guid.NewGuid().ToString("N");
            entry.Modified = Now;
            deleted.RemoveAll(x => x.Id == entry.Id);
            entries.Add(entry);
            return true;
        }
    }

    public bool Update(MyPassword original, MyPassword edited)
    {
        lock (sync)
        {
            EnsureUnlocked();
            if (entries.Any(x => x.Id != original.Id && x.HasSameContent(edited)))
                return false;

            edited.Id = string.IsNullOrEmpty(original.Id) ? Guid.NewGuid().ToString("N") : original.Id;
            // Strictly later than the version it replaces, even within the same clock tick.
            DateTimeOffset now = Now;
            edited.Modified = now > original.Modified ? now : original.Modified.AddTicks(1);

            int index = entries.FindIndex(x => x.Id == edited.Id);
            if (index >= 0)
                entries[index] = edited;
            else
                entries.Add(edited); // Deleted meanwhile (by a merge): the edit brings it back.
            deleted.RemoveAll(x => x.Id == edited.Id);
            return true;
        }
    }

    public void Remove(MyPassword entry)
    {
        lock (sync)
        {
            EnsureUnlocked();
            if (entries.RemoveAll(x => ReferenceEquals(x, entry) || (x.Id.Length > 0 && x.Id == entry.Id)) == 0)
                return;
            if (entry.Id.Length > 0)
            {
                deleted.RemoveAll(x => x.Id == entry.Id);
                deleted.Add(new VaultTombstone { Id = entry.Id, At = Now });
            }
        }
    }

    public void ReplaceAll(IEnumerable<MyPassword> newEntries)
    {
        lock (sync)
        {
            EnsureUnlocked();
            List<MyPassword> current = [.. entries];
            entries.Clear();
            foreach (MyPassword entry in newEntries)
            {
                if (entries.Any(entry.HasSameContent))
                    continue;
                MyPassword? existing = current.FirstOrDefault(x => x.Id == entry.Id || x.HasSameContent(entry));
                if (existing is null || !existing.HasSameContent(entry))
                {
                    if (string.IsNullOrEmpty(entry.Id))
                        entry.Id = Guid.NewGuid().ToString("N");
                    entry.Modified = Now;
                }
                entries.Add(entry);
            }

            DateTimeOffset now = Now;
            foreach (MyPassword removed in current.Where(x => !entries.Any(y => y.Id == x.Id)))
                deleted.Add(new VaultTombstone { Id = removed.Id, At = now });
        }
    }

    public IReadOnlyList<MyPassword> Search(string? text)
    {
        lock (sync)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Trim() == "*")
                return [.. entries];

            string term = text.Trim();
            return [.. entries.Where(x => Matches(x, term))];
        }
    }

    private static bool Matches(MyPassword entry, string term) =>
        entry.Data.Contains(term, StringComparison.OrdinalIgnoreCase)
        || entry.UserName.Contains(term, StringComparison.OrdinalIgnoreCase)
        || entry.Keyword.Contains(term, StringComparison.OrdinalIgnoreCase);
    #endregion

    #region Read / write
    public Task SaveAsync(CancellationToken cancellationToken = default) =>
        Task.Run(Save, cancellationToken);

    /// <inheritdoc cref="IVaultService.SaveAsync"/>
    /// <exception cref="InvalidOperationException">Locked, or the file was replaced by a vault with another password (now locked).</exception>
    public void Save()
    {
        EnsureUnlocked();
        lock (fileSync)
        {
            MergeExternalChanges();
            WriteWithCurrentKey(Snapshot());
        }
        Saved?.Invoke(this, EventArgs.Empty);
    }

    public Task ReloadAsync(CancellationToken cancellationToken = default) =>
        Task.Run(Reload, cancellationToken);

    /// <inheritdoc cref="IVaultService.ReloadAsync"/>
    /// <exception cref="InvalidOperationException">Locked, or the file was replaced by a vault with another password (now locked).</exception>
    /// <exception cref="CryptographicException">The file is corrupted or was modified.</exception>
    public void Reload()
    {
        EnsureUnlocked();
        lock (fileSync)
            MergeExternalChanges();
    }

    public async Task<bool> DownloadAsync(string url, CancellationToken cancellationToken = default) =>
        ImportVaultFile(await VaultDownloader.DownloadAsync(url, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// True if <paramref name="content"/> looks like a 3.x data file (one base64 line per entry),
    /// as published on a web server for the 3.x "update from URL" feature.
    /// </summary>
    public static bool IsLegacyDataFile(byte[] content)
    {
        if (content.Length == 0 || VaultCipher.IsVaultFile(content))
            return false;

        string[] lines = LegacyLines(content);
        return lines.Length > 0 && lines.All(line => line.Length % 4 == 0 && Convert.TryFromBase64String(line, new byte[line.Length], out _));
    }

    private static string[] LegacyLines(byte[] content) =>
        [.. Encoding.UTF8.GetString(content).Split('\n').Select(x => x.Trim()).Where(x => x.Length > 0)];

    public Task<bool> CreateFromLegacyDataAsync(byte[] content, string password, CancellationToken cancellationToken = default) =>
        Task.Run(() => CreateFromLegacyData(content, password), cancellationToken);

    /// <inheritdoc cref="IVaultService.CreateFromLegacyDataAsync"/>
    public bool CreateFromLegacyData(byte[] content, string password)
    {
        if (IsInitialized)
            throw new InvalidOperationException("A vault already exists. Erase it first.");
        if (!IsLegacyDataFile(content))
            throw new VaultFormatException("The file is not a Password Vault 3.x data file.");

        if (ReadLegacyData(content, password) is not List<MyPassword> entries)
            return false;

        Lock();
        VaultKdfParameters newKdf = VaultCipher.NewKdfParameters(options.Pbkdf2Iterations);
        byte[] newKey = VaultCipher.DeriveKey(password, newKdf);
        VaultPayload payload = VaultMerge.Normalize(new VaultPayload { Entries = entries }, Now);
        lock (fileSync)
        {
            WriteVault(payload, newKey, newKdf);
            lock (sync)
            {
                key = newKey;
                kdf = newKdf;
                SetData(payload);
            }
        }
        Saved?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public Task<int?> ImportLegacyDataAsync(byte[] content, string password, CancellationToken cancellationToken = default) =>
        Task.Run(() => ImportLegacyData(content, password), cancellationToken);

    /// <inheritdoc cref="IVaultService.ImportLegacyDataAsync"/>
    public int? ImportLegacyData(byte[] content, string password)
    {
        EnsureUnlocked();
        if (!IsLegacyDataFile(content))
            throw new VaultFormatException("The file is not a Password Vault 3.x data file.");
        if (ReadLegacyData(content, password) is not List<MyPassword> found)
            return null;

        int added = 0;
        foreach (MyPassword entry in found)
        {
            // Same id on every device importing the same old file: merging later does not duplicate the entry.
            entry.Id = MyPassword.ContentId(entry);
            if (Add(entry))
                added++;
        }

        Save();
        return added;
    }

    /// <summary>
    /// Entries of a 3.x data file, or null if the password is wrong. No key file comes with such a file:
    /// the password is checked by decrypting every entry.
    /// </summary>
    private static List<MyPassword>? ReadLegacyData(byte[] content, string password)
    {
        List<MyPassword> entries = [];
        try
        {
            foreach (string json in LegacyV1Crypto.DecryptDataLines(password, LegacyLines(content)))
            {
                if (MyPassword.From(json) is not MyPassword entry)
                    return null;
                entries.Add(entry);
            }
        }
        catch (Exception exception) when (exception is CryptographicException or JsonException or FormatException or DecoderFallbackException)
        {
            return null;
        }
        return entries;
    }

    /// <inheritdoc cref="IVaultService.ImportVaultFile"/>
    /// <exception cref="VaultFormatException">Not a v2 vault file (for example an old v1 file).</exception>
    public bool ImportVaultFile(byte[] content)
    {
        VaultEnvelope envelope = VaultCipher.Parse(content);

        lock (fileSync)
        {
            byte[]? currentKey = CloneKeyFor(envelope.Kdf);
            byte[] plaintext = [];
            bool opened = currentKey is not null && VaultCipher.TryOpen(envelope, currentKey, out plaintext);
            if (currentKey is not null)
                CryptographicOperations.ZeroMemory(currentKey);

            storage.Write(content);
            lastFileHash = SHA256.HashData(content);

            if (!opened)
            {
                Lock();
                return false;
            }

            VaultPayload loaded = VaultMerge.Normalize(DeserializePayload(plaintext), Now);
            lock (sync)
                SetData(loaded);
        }
        return true;
    }

    public byte[] ReadVaultFile()
    {
        lock (fileSync)
            return storage.Read();
    }
    #endregion

    #region Internals
    private void EnsureUnlocked()
    {
        lock (sync)
        {
            if (key is null || kdf is null)
                throw new InvalidOperationException("The vault is locked.");
        }
    }

    /// <summary>Copy of the entries and deletions (caller must not hold <see cref="sync"/>).</summary>
    private VaultPayload Snapshot()
    {
        lock (sync)
            return new VaultPayload { Entries = [.. entries], Deleted = [.. deleted] };
    }

    /// <summary>Replace the in-memory state (caller holds <see cref="sync"/>).</summary>
    private void SetData(VaultPayload payload)
    {
        entries.Clear();
        entries.AddRange(payload.Entries);
        deleted.Clear();
        deleted.AddRange(payload.Deleted);
    }

    private byte[]? CloneKeyFor(VaultKdfParameters fileKdf)
    {
        lock (sync)
            return fileKdf.SameAs(kdf) ? (byte[])key!.Clone() : null;
    }

    /// <summary>
    /// If the file changed since it was last read or written by this instance, merge it into memory.
    /// Caller holds <see cref="fileSync"/>.
    /// </summary>
    private void MergeExternalChanges()
    {
        if (!storage.Exists())
            return;

        byte[] content = storage.Read();
        byte[] hash = SHA256.HashData(content);
        if (lastFileHash is not null && CryptographicOperations.FixedTimeEquals(hash, lastFileHash))
            return;

        VaultEnvelope envelope = VaultCipher.Parse(content);
        byte[]? openKey = CloneKeyFor(envelope.Kdf);
        if (openKey is null)
        {
            // Replaced by a vault with another salt/password: the key in memory no longer applies.
            Lock();
            throw new InvalidOperationException("The vault file was replaced. Unlock it again.");
        }

        bool opened = VaultCipher.TryOpen(envelope, openKey, out byte[] plaintext);
        CryptographicOperations.ZeroMemory(openKey);
        if (!opened)
            throw new CryptographicException("The vault file is corrupted or was modified.");

        VaultPayload merged = VaultMerge.Merge(Snapshot(), DeserializePayload(plaintext), Now);
        lock (sync)
            SetData(merged);
        lastFileHash = hash;
    }

    /// <summary>Encrypt with the current key and write. Caller holds <see cref="fileSync"/>.</summary>
    private void WriteWithCurrentKey(VaultPayload payload)
    {
        byte[] writeKey;
        VaultKdfParameters writeKdf;
        lock (sync)
        {
            if (key is null || kdf is null)
                throw new InvalidOperationException("The vault is locked.");
            // Copy: Lock() may zero the original while we are still writing.
            writeKey = (byte[])key.Clone();
            writeKdf = kdf;
        }

        try
        {
            WriteVault(payload, writeKey, writeKdf);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(writeKey);
        }
    }

    /// <summary>Caller holds <see cref="fileSync"/>.</summary>
    private void WriteVault(VaultPayload payload, byte[] writeKey, VaultKdfParameters writeKdf)
    {
        byte[] plaintext = JsonSerializer.SerializeToUtf8Bytes(VaultMerge.Normalize(payload, Now), VaultJsonContext.Default.VaultPayload);
        try
        {
            byte[] content = VaultCipher.Seal(plaintext, writeKey, writeKdf);
            storage.Write(content);
            lastFileHash = SHA256.HashData(content);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(plaintext);
        }
    }

    private static VaultPayload DeserializePayload(byte[] plaintext)
    {
        try
        {
            return JsonSerializer.Deserialize(plaintext, VaultJsonContext.Default.VaultPayload) ?? new VaultPayload();
        }
        finally
        {
            CryptographicOperations.ZeroMemory(plaintext);
        }
    }

    /// <summary>
    /// Convert a v1 vault (key file + line-encrypted data file) to v2, then delete the old files.
    /// </summary>
    private bool MigrateFromV1(string password)
    {
        string keyPath = options.LegacyKeyFilePath!;
        if (!LegacyV1Crypto.CheckPassword(password, File.ReadAllText(keyPath)))
            return false;

        List<MyPassword> migrated = [];
        string? dataPath = options.LegacyDataFilePath;
        if (dataPath is not null && File.Exists(dataPath))
        {
            foreach (string json in LegacyV1Crypto.DecryptDataLines(password, File.ReadAllLines(dataPath)))
            {
                MyPassword? entry = MyPassword.From(json);
                if (entry is not null)
                    migrated.Add(entry);
            }
        }

        VaultKdfParameters newKdf = VaultCipher.NewKdfParameters(options.Pbkdf2Iterations);
        byte[] newKey = VaultCipher.DeriveKey(password, newKdf);
        VaultPayload payload = VaultMerge.Normalize(new VaultPayload { Entries = migrated }, Now);

        lock (fileSync)
        {
            // Write v2 first: if anything fails after this point, the next unlock simply uses the v2 file.
            WriteVault(payload, newKey, newKdf);

            File.Delete(keyPath);
            if (dataPath is not null)
                File.Delete(dataPath);

            lock (sync)
            {
                key = newKey;
                kdf = newKdf;
                SetData(payload);
            }
        }
        return true;
    }
    #endregion
}
