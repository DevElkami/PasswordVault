using VaultCore.Crypto;

namespace VaultCore;

/// <summary>
/// File locations and crypto settings for a <see cref="VaultService"/>.
/// </summary>
public sealed class VaultOptions
{
    public const string VaultFileName = "my.vault";
    public const string LegacyKeyFileName = "VaultCore.key";
    public const string LegacyDataFileName = "SimpleDb.sqlite";

    /// <summary>The v2 encrypted vault file.</summary>
    public required string VaultFilePath { get; init; }

    /// <summary>v1 key file, only read to migrate an old vault. Null disables migration.</summary>
    public string? LegacyKeyFilePath { get; init; }

    /// <summary>v1 data file, only read to migrate an old vault.</summary>
    public string? LegacyDataFilePath { get; init; }

    /// <summary>PBKDF2 iterations used when creating a vault or changing its password.</summary>
    public int Pbkdf2Iterations { get; init; } = VaultCipher.DefaultIterations;

    /// <summary>Clock used to date changes (replaceable in tests).</summary>
    public TimeProvider TimeProvider { get; init; } = TimeProvider.System;

    /// <summary>
    /// Default locations. The v2 file sits next to where the v1 data file used to be.
    /// Debug desktop builds keep everything next to the executable, as before, so development
    /// never touches the real vault.
    /// </summary>
    public static VaultOptions CreateDefault()
    {
#if DEBUG
        if (!OperatingSystem.IsAndroid() && !OperatingSystem.IsIOS())
            return ForDirectory(AppContext.BaseDirectory);
#endif
        string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return new VaultOptions
        {
            VaultFilePath = Path.Combine(documents, VaultFileName),
            LegacyKeyFilePath = Path.Combine(localAppData, LegacyKeyFileName),
            LegacyDataFilePath = Path.Combine(documents, LegacyDataFileName),
        };
    }

    /// <summary>All files (v2 and legacy v1) in one directory.</summary>
    public static VaultOptions ForDirectory(string directory, int iterations = VaultCipher.DefaultIterations) => new()
    {
        VaultFilePath = Path.Combine(directory, VaultFileName),
        LegacyKeyFilePath = Path.Combine(directory, LegacyKeyFileName),
        LegacyDataFilePath = Path.Combine(directory, LegacyDataFileName),
        Pbkdf2Iterations = iterations,
    };
}
