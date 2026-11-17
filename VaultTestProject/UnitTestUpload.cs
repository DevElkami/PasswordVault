using VaultCore.Upload;

namespace VaultTestProject;

[TestClass]
public class UnitTestUpload
{
    [TestMethod(DisplayName = "Upload address: protocols, default ports, path")]
    [DataRow("sftp://example.com/www/my.vault", UploadProtocol.Sftp, "example.com", 22, "/www/my.vault", false)]
    [DataRow("  SFTP://Example.com:2222/a/b.vault ", UploadProtocol.Sftp, "example.com", 2222, "/a/b.vault", false)]
    [DataRow("ftps://example.com/my.vault", UploadProtocol.Ftps, "example.com", 21, "/my.vault", false)]
    [DataRow("ftp://example.com/dir/x.vault", UploadProtocol.Ftps, "example.com", 21, "/dir/x.vault", false)]
    [DataRow("ftpes://example.com:2121/x.vault", UploadProtocol.Ftps, "example.com", 2121, "/x.vault", false)]
    [DataRow("ftps://example.com:990/x.vault", UploadProtocol.Ftps, "example.com", 990, "/x.vault", true)]
    [DataRow("sftp://example.com/mes%20fichiers/coffre.vault", UploadProtocol.Sftp, "example.com", 22, "/mes fichiers/coffre.vault", false)]
    public void ParseAddress(string text, UploadProtocol protocol, string host, int port, string path, bool implicitTls)
    {
        UploadAddress address = UploadAddress.Parse(text);
        Assert.AreEqual(protocol, address.Protocol);
        Assert.AreEqual(host, address.Host);
        Assert.AreEqual(port, address.Port);
        Assert.AreEqual(path, address.Path);
        Assert.AreEqual(implicitTls, address.ImplicitTls);
    }

    [TestMethod(DisplayName = "Upload address: refused forms")]
    [DataRow("")]
    [DataRow("example.com/file.vault")]
    [DataRow("https://example.com/file.vault")]
    [DataRow("sftp://example.com")]
    [DataRow("sftp://example.com/")]
    [DataRow("sftp://example.com/folder/")]
    [DataRow("sftp://user:secret@example.com/file.vault")]
    [DataRow("sftp://user@example.com/file.vault")]
    [DataRow("ftps://example.com/file.vault?x=1")]
    public void RefuseAddress(string text) =>
        Assert.ThrowsExactly<FormatException>(() => UploadAddress.Parse(text));

    [TestMethod(DisplayName = "Server identity: first time, known, changed, other server")]
    public void Pin()
    {
        UploadAddress address = UploadAddress.Parse("sftp://example.com/x.vault");
        ServerIdentity identity = new("SHA256:aaaa", "ssh-ed25519");

        Assert.AreEqual(ServerTrust.FirstTime, ServerPin.Evaluate(null, address, identity));
        Assert.AreEqual(ServerTrust.FirstTime, ServerPin.Evaluate("", address, identity));

        string pin = ServerPin.Create(address, identity);
        Assert.AreEqual(ServerTrust.Known, ServerPin.Evaluate(pin, address, identity));
        // Same server, another file: still known.
        Assert.AreEqual(ServerTrust.Known, ServerPin.Evaluate(pin, UploadAddress.Parse("sftp://EXAMPLE.com/other/y.vault"), identity));

        Assert.AreEqual(ServerTrust.Changed, ServerPin.Evaluate(pin, address, new ServerIdentity("SHA256:bbbb", "ssh-ed25519")));

        // Another server, port or protocol is a first contact, never "known".
        Assert.AreEqual(ServerTrust.FirstTime, ServerPin.Evaluate(pin, UploadAddress.Parse("sftp://example.com:2222/x.vault"), identity));
        Assert.AreEqual(ServerTrust.FirstTime, ServerPin.Evaluate(pin, UploadAddress.Parse("ftps://example.com/x.vault"), identity));
        Assert.AreEqual(ServerTrust.FirstTime, ServerPin.Evaluate(pin, UploadAddress.Parse("sftp://example.com.evil.test/x.vault"), identity));
    }
}
