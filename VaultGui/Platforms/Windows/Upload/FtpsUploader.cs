using System.Net.Security;
using System.Security.Cryptography;
using FluentFTP;
using VaultCore.Upload;

namespace VaultGui.Platforms.Windows.Upload;

/// <summary>
/// Publishes the vault file over FTP with TLS (explicit "AUTH TLS", or implicit on port 990).
/// Never falls back to unencrypted FTP: if the server has no TLS, the connection fails before the login is sent.
/// No MAUI dependency: usable from a test harness.
/// </summary>
public static class FtpsUploader
{
    private const int TimeoutMilliseconds = 20_000;

    /// <param name="trust">Called when Windows does not trust the server certificate (self-signed, wrong name, expired);
    /// return false to abort before the password is sent. Not called for a certificate Windows trusts.</param>
    /// <exception cref="ServerNotTrustedException">The certificate was refused by <paramref name="trust"/>.</exception>
    public static void Upload(UploadAddress address, string user, string password, byte[] content, Func<ServerIdentity, bool> trust)
    {
        using FtpClient client = Connect(address, user, password, trust);

        // Upload next to the target, then swap: a download during the transfer never gets half a file.
        string temp = address.Path + TempSuffix;
        FtpStatus status = client.UploadBytes(content, temp, FtpRemoteExists.Overwrite, createRemoteDir: false);
        if (status != FtpStatus.Success)
            throw new IOException("The server refused the file.");

        long size = client.GetFileSize(temp);
        if (size >= 0 && size != content.Length)
            throw new IOException("The server did not store the whole file.");

        if (!client.MoveFile(temp, address.Path, FtpRemoteExists.Overwrite))
            throw new IOException("The server refused to replace the file.");

        client.Disconnect();
    }

    /// <summary>
    /// Remove the published file (and what an interrupted upload left) from the server.
    /// A file that is already gone is not an error.
    /// </summary>
    /// <inheritdoc cref="Upload" path="/param"/>
    /// <exception cref="ServerNotTrustedException">The certificate was refused by <paramref name="trust"/>.</exception>
    public static void Delete(UploadAddress address, string user, string password, Func<ServerIdentity, bool> trust)
    {
        using FtpClient client = Connect(address, user, password, trust);

        foreach (string path in new[] { address.Path, address.Path + TempSuffix })
        {
            if (client.FileExists(path))
                client.DeleteFile(path);
        }

        if (client.FileExists(address.Path))
            throw new IOException("The server did not delete the file.");

        client.Disconnect();
    }

    private const string TempSuffix = ".uploading";

    private static FtpClient Connect(UploadAddress address, string user, string password, Func<ServerIdentity, bool> trust)
    {
        FtpClient client = new(address.Host, user, password, address.Port);
        client.Config.EncryptionMode = address.ImplicitTls ? FtpEncryptionMode.Implicit : FtpEncryptionMode.Explicit;
        client.Config.DataConnectionEncryption = true;
        client.Config.ValidateAnyCertificate = false;
        client.Config.ConnectTimeout = TimeoutMilliseconds;
        client.Config.ReadTimeout = TimeoutMilliseconds;
        client.Config.DataConnectionConnectTimeout = TimeoutMilliseconds;
        client.Config.DataConnectionReadTimeout = TimeoutMilliseconds;
        client.Config.RetryAttempts = 1;

        bool refused = false;
        client.ValidateCertificate += (_, e) =>
        {
            if (e.PolicyErrors == SslPolicyErrors.None)
            {
                e.Accept = true;
                return;
            }

            string fingerprint = "SHA256:" + Convert.ToBase64String(SHA256.HashData(e.Certificate.GetRawCertData())).TrimEnd('=');
            e.Accept = trust(new ServerIdentity(fingerprint, $"{e.Certificate.Subject} ({e.PolicyErrors})"));
            refused = !e.Accept;
        };

        try
        {
            client.Connect();
            return client;
        }
        catch (Exception)
        {
            client.Dispose();
            if (refused)
                throw new ServerNotTrustedException();
            throw;
        }
    }
}
