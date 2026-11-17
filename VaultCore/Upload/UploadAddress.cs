namespace VaultCore.Upload;

public enum UploadProtocol
{
    /// <summary>File transfer over SSH.</summary>
    Sftp,

    /// <summary>FTP over TLS. Plain, unencrypted FTP is never used.</summary>
    Ftps,
}

/// <summary>
/// Where the vault file is published: <c>sftp://host[:port]/path/file</c> or <c>ftps://host[:port]/path/file</c>.
/// <para>
/// <c>ftp://</c> and <c>ftpes://</c> are accepted as synonyms of <c>ftps://</c>: the connection always
/// requires TLS (explicit, or implicit on port 990), so the login never travels in clear text.
/// </para>
/// </summary>
public sealed record UploadAddress(UploadProtocol Protocol, string Host, int Port, string Path)
{
    public const int SftpPort = 22;
    public const int FtpPort = 21;
    public const int FtpImplicitTlsPort = 990;

    /// <summary>FTPS only: TLS from the first byte (port 990) instead of "AUTH TLS" on the FTP port.</summary>
    public bool ImplicitTls => Protocol == UploadProtocol.Ftps && Port == FtpImplicitTlsPort;

    /// <summary>Identifies the server (not the file) when remembering its identity.</summary>
    public string ServerKey => $"{(Protocol == UploadProtocol.Sftp ? "sftp" : "ftps")}://{Host.ToLowerInvariant()}:{Port}";

    public override string ToString() => ServerKey + Path;

    /// <exception cref="FormatException">Not a usable address (the message says why).</exception>
    public static UploadAddress Parse(string? text)
    {
        if (!Uri.TryCreate(text?.Trim(), UriKind.Absolute, out Uri? uri))
            throw new FormatException("The address must look like sftp://example.com/folder/my.vault.");

        UploadProtocol protocol = uri.Scheme.ToLowerInvariant() switch
        {
            "sftp" => UploadProtocol.Sftp,
            "ftp" or "ftps" or "ftpes" => UploadProtocol.Ftps,
            _ => throw new FormatException("The address must start with sftp:// or ftps://."),
        };

        // A password in the address would be stored and displayed in clear text.
        if (uri.UserInfo.Length > 0)
            throw new FormatException("Enter the user name and password in their own fields, not in the address.");
        if (uri.Host.Length == 0)
            throw new FormatException("The server name is missing.");
        if (uri.Query.Length > 0 || uri.Fragment.Length > 0)
            throw new FormatException("The address must not contain '?' or '#'.");

        string path = Uri.UnescapeDataString(uri.AbsolutePath);
        if (path.Length <= 1 || path.EndsWith('/'))
            throw new FormatException("The address must end with the file name, for example /folder/my.vault.");

        int port = uri.Port > 0 ? uri.Port : protocol == UploadProtocol.Sftp ? SftpPort : FtpPort;
        return new UploadAddress(protocol, uri.Host, port, path);
    }
}
