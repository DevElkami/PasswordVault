using VaultCore;

namespace VaultTestProject;

/// <summary>
/// Isolated temporary directory holding one test vault. Deleted on dispose.
/// </summary>
internal sealed class TestVaultDirectory : IDisposable
{
    /// <summary>Low iteration count so tests stay fast. Production uses VaultCipher.DefaultIterations.</summary>
    public const int FastIterations = 1_000;

    public string Path { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "PasswordVaultTests", Guid.NewGuid().ToString("N"));

    public TestVaultDirectory() => Directory.CreateDirectory(Path);

    public VaultOptions Options => VaultOptions.ForDirectory(Path, FastIterations);

    public VaultService NewVault() => new(Options);

    public void Dispose()
    {
        try { Directory.Delete(Path, recursive: true); }
        catch (IOException) { }
    }
}
