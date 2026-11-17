using System.Text.Json.Serialization;
using VaultCore.Models;

namespace VaultCore.Crypto;

/// <summary>On-disk JSON layout of a v2 vault file.</summary>
internal sealed class VaultFileModel
{
    [JsonPropertyName("format")] public string? Format { get; set; }
    [JsonPropertyName("version")] public int Version { get; set; }
    [JsonPropertyName("kdf")] public VaultFileKdf? Kdf { get; set; }
    [JsonPropertyName("cipher")] public VaultFileCipher? Cipher { get; set; }
    [JsonPropertyName("payload")] public string? Payload { get; set; }
}

internal sealed class VaultFileKdf
{
    [JsonPropertyName("algorithm")] public string? Algorithm { get; set; }
    [JsonPropertyName("iterations")] public int Iterations { get; set; }
    [JsonPropertyName("salt")] public string? Salt { get; set; }
}

internal sealed class VaultFileCipher
{
    [JsonPropertyName("algorithm")] public string? Algorithm { get; set; }
    [JsonPropertyName("nonce")] public string? Nonce { get; set; }
    [JsonPropertyName("tag")] public string? Tag { get; set; }
}

/// <summary>Decrypted content of a vault file.</summary>
internal sealed class VaultPayload
{
    [JsonPropertyName("entries")] public List<MyPassword> Entries { get; set; } = [];

    /// <summary>Deleted entry ids, so merging with an older version of the file does not bring them back.</summary>
    [JsonPropertyName("deleted")] public List<VaultTombstone> Deleted { get; set; } = [];
}

internal sealed class VaultTombstone
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("at")] public DateTimeOffset At { get; set; }
}

/// <summary>
/// Source-generated JSON metadata: no reflection, safe for trimmed Android/AOT builds.
/// </summary>
[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(VaultFileModel))]
[JsonSerializable(typeof(VaultPayload))]
[JsonSerializable(typeof(MyPassword))]
internal partial class VaultJsonContext : JsonSerializerContext
{
}
