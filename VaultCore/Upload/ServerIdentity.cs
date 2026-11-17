namespace VaultCore.Upload;

/// <summary>
/// What a server presents to prove who it is: its SSH host key (SFTP) or its TLS certificate (FTPS).
/// </summary>
/// <param name="Fingerprint">"SHA256:..." of the host key or certificate.</param>
/// <param name="Description">Key type, or certificate subject and why it is not trusted by Windows.</param>
public sealed record ServerIdentity(string Fingerprint, string Description);

public enum ServerTrust
{
    /// <summary>Same identity as the one accepted before.</summary>
    Known,

    /// <summary>Never seen this server: the user must confirm its fingerprint.</summary>
    FirstTime,

    /// <summary>The server no longer presents the identity accepted before (reinstalled, or impersonated).</summary>
    Changed,
}

/// <summary>
/// "Trust on first use": the identity accepted by the user is remembered ("pinned") and every later
/// connection must present the same one. Needed because an SSH host key or a self-signed certificate
/// cannot be checked any other way.
/// </summary>
public static class ServerPin
{
    public static string Create(UploadAddress address, ServerIdentity identity) => address.ServerKey + "|" + identity.Fingerprint;

    public static ServerTrust Evaluate(string? pin, UploadAddress address, ServerIdentity identity)
    {
        if (string.IsNullOrEmpty(pin) || !pin.StartsWith(address.ServerKey + "|", StringComparison.Ordinal))
            return ServerTrust.FirstTime;

        return pin == Create(address, identity) ? ServerTrust.Known : ServerTrust.Changed;
    }
}

/// <summary>The identity presented by the server was not accepted: nothing was sent, not even the login.</summary>
public sealed class ServerNotTrustedException() : Exception("The server identity was not accepted.");

public enum UploadFailure
{
    /// <summary>User name or password refused.</summary>
    Authentication,

    /// <summary>The server offers no encrypted connection.</summary>
    NoEncryption,
}

/// <summary>A failure the app can explain in the user's language.</summary>
public sealed class UploadFailedException(UploadFailure reason, Exception inner) : Exception(inner.Message, inner)
{
    public UploadFailure Reason { get; } = reason;
}
