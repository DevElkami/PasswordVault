using VaultGui.Services;
using VaultCore.Upload;

namespace VaultGui.Platforms.Windows.Upload;

public sealed class WindowsVaultUploader : IVaultUploader
{
    public bool IsSupported => true;

    public Task UploadAsync(UploadAddress address, string user, string password, byte[] content, Func<ServerIdentity, bool> trust) =>
        RunAsync(() =>
        {
            if (address.Protocol == UploadProtocol.Sftp)
                SftpUploader.Upload(address, user, password, content, trust);
            else
                FtpsUploader.Upload(address, user, password, content, trust);
        });

    public Task DeleteAsync(UploadAddress address, string user, string password, Func<ServerIdentity, bool> trust) =>
        RunAsync(() =>
        {
            if (address.Protocol == UploadProtocol.Sftp)
                SftpUploader.Delete(address, user, password, trust);
            else
                FtpsUploader.Delete(address, user, password, trust);
        });

    private static Task RunAsync(Action transfer) =>
        Task.Run(() =>
        {
            try
            {
                transfer();
            }
            catch (Exception exception) when (exception is Renci.SshNet.Common.SshAuthenticationException or FluentFTP.Exceptions.FtpAuthenticationException)
            {
                throw new UploadFailedException(UploadFailure.Authentication, exception);
            }
            catch (FluentFTP.Exceptions.FtpSecurityNotAvailableException exception)
            {
                throw new UploadFailedException(UploadFailure.NoEncryption, exception);
            }
        });
}
