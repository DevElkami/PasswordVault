using VaultCore.Upload;

namespace VaultGui.Services;

/// <summary>
/// Platform part of "send the vault to your server" (SFTP / FTP over TLS). Windows only:
/// the PC is where the vault is edited, other devices download the published file over https.
/// </summary>
public interface IVaultUploader
{
    bool IsSupported { get; }

    /// <param name="trust">Decides, from the identity the server presents, whether to go on. Must answer at once
    /// (no user prompt): it is called in the middle of the connection handshake.</param>
    /// <exception cref="ServerNotTrustedException"><paramref name="trust"/> returned false.</exception>
    /// <exception cref="UploadFailedException">Login refused, or no encryption available.</exception>
    Task UploadAsync(UploadAddress address, string user, string password, byte[] content, Func<ServerIdentity, bool> trust);

    /// <summary>Remove the published file from the server (no error if it is already gone).</summary>
    /// <inheritdoc cref="UploadAsync"/>
    Task DeleteAsync(UploadAddress address, string user, string password, Func<ServerIdentity, bool> trust);
}

public sealed class NoVaultUploader : IVaultUploader
{
    public bool IsSupported => false;

    public Task UploadAsync(UploadAddress address, string user, string password, byte[] content, Func<ServerIdentity, bool> trust) =>
        throw new PlatformNotSupportedException();

    public Task DeleteAsync(UploadAddress address, string user, string password, Func<ServerIdentity, bool> trust) =>
        throw new PlatformNotSupportedException();
}
