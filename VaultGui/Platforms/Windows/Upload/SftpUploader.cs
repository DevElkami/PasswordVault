using Renci.SshNet;
using Renci.SshNet.Common;
using VaultCore.Upload;

namespace VaultGui.Platforms.Windows.Upload;

/// <summary>
/// Publishes the vault file over SFTP (SSH). No MAUI dependency: usable from a test harness.
/// </summary>
public static class SftpUploader
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(20);

    /// <param name="trust">Called with the server's host key; return false to abort before the password is sent.</param>
    /// <exception cref="ServerNotTrustedException">The host key was refused by <paramref name="trust"/>.</exception>
    public static void Upload(UploadAddress address, string user, string password, byte[] content, Func<ServerIdentity, bool> trust)
    {
        using SftpClient client = Connect(address, user, password, trust);

        // Upload next to the target, then swap: a download during the transfer never gets half a file.
        string temp = address.Path + TempSuffix;
        using (MemoryStream stream = new(content))
            client.UploadFile(stream, temp, canOverride: true);

        if (client.GetAttributes(temp).Size != content.Length)
            throw new IOException("The server did not store the whole file.");

        try
        {
            // OpenSSH extension: atomic replace.
            client.RenameFile(temp, address.Path, isPosix: true);
        }
        catch (NotSupportedException)
        {
            if (client.Exists(address.Path))
                client.DeleteFile(address.Path);
            client.RenameFile(temp, address.Path);
        }

        client.Disconnect();
    }

    /// <summary>
    /// Remove the published file (and what an interrupted upload left) from the server.
    /// A file that is already gone is not an error.
    /// </summary>
    /// <inheritdoc cref="Upload" path="/param"/>
    /// <exception cref="ServerNotTrustedException">The host key was refused by <paramref name="trust"/>.</exception>
    public static void Delete(UploadAddress address, string user, string password, Func<ServerIdentity, bool> trust)
    {
        using SftpClient client = Connect(address, user, password, trust);

        foreach (string path in new[] { address.Path, address.Path + TempSuffix })
        {
            if (client.Exists(path))
                client.DeleteFile(path);
        }

        if (client.Exists(address.Path))
            throw new IOException("The server did not delete the file.");

        client.Disconnect();
    }

    private const string TempSuffix = ".uploading";

    private static SftpClient Connect(UploadAddress address, string user, string password, Func<ServerIdentity, bool> trust)
    {
        // Some servers only offer "keyboard-interactive": answer its prompts with the password.
        KeyboardInteractiveAuthenticationMethod interactive = new(user);
        interactive.AuthenticationPrompt += (_, e) =>
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
                prompt.Response = password;
        };

        ConnectionInfo connection = new(address.Host, address.Port, user, new PasswordAuthenticationMethod(user, password), interactive)
        {
            Timeout = Timeout,
        };

        SftpClient client = new(connection);
        client.OperationTimeout = Timeout;

        bool refused = false;
        client.HostKeyReceived += (_, e) =>
        {
            e.CanTrust = trust(new ServerIdentity("SHA256:" + e.FingerPrintSHA256, e.HostKeyName));
            refused = !e.CanTrust;
        };

        try
        {
            client.Connect();
            return client;
        }
        catch (Exception exception)
        {
            client.Dispose();
            if (exception is SshConnectionException && refused)
                throw new ServerNotTrustedException();
            throw;
        }
    }
}
