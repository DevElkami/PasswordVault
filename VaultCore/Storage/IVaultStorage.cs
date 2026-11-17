namespace VaultCore.Storage;

/// <summary>
/// Where the encrypted vault file lives. Lets each platform (or a future sync provider)
/// decide on the location without touching the vault logic.
/// </summary>
public interface IVaultStorage
{
    bool Exists();
    byte[] Read();

    /// <summary>Replace the vault content. Must be atomic: never leave a half-written file.</summary>
    void Write(byte[] content);

    void Delete();
}

/// <summary>
/// Vault stored as a single local file.
/// </summary>
public sealed class FileVaultStorage(string path) : IVaultStorage
{
    public string Path { get; } = path;

    public bool Exists() => File.Exists(Path);

    public byte[] Read() => File.ReadAllBytes(Path);

    public void Write(byte[] content)
    {
        string? directory = System.IO.Path.GetDirectoryName(Path);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        string temp = $"{Path}.{Guid.NewGuid():N}.tmp";
        File.WriteAllBytes(temp, content);
        try
        {
            ReplaceWithRetry(temp, Path);
        }
        catch
        {
            File.Delete(temp);
            throw;
        }
    }

    /// <summary>
    /// Antivirus, indexer or sync tools (OneDrive...) can hold a just-written file for a few milliseconds.
    /// </summary>
    private static void ReplaceWithRetry(string source, string destination)
    {
        const int attempts = 10;
        for (int attempt = 1; ; attempt++)
        {
            try
            {
                File.Move(source, destination, overwrite: true);
                return;
            }
            catch (Exception exception) when (attempt < attempts && exception is IOException or UnauthorizedAccessException)
            {
                Thread.Sleep(50 * attempt);
            }
        }
    }

    public void Delete() => File.Delete(Path);
}
