using VaultCore;
using VaultCore.Models;

namespace VaultTestProject;

[TestClass]
public class UnitTestMyPassword
{
    /// <summary>Old vault files can hold null fields: they are read as empty strings.</summary>
    [TestMethod]
    public void NullFieldsAreReadAsEmpty()
    {
        MyPassword? entry = MyPassword.From("""{"Data":"https://example.com","UserName":"me","Password":null,"Keyword":null,"Id":null}""");

        Assert.IsNotNull(entry);
        Assert.AreEqual("https://example.com", entry.Data);
        Assert.AreEqual("", entry.Password);
        Assert.AreEqual("", entry.Keyword);
        Assert.AreEqual("", entry.Id);
        Assert.AreEqual("", entry.KeywordLine);
        Assert.AreEqual("example.com", VaultDuplicates.SiteKey(entry));
    }

    [TestMethod]
    public void SettingNullStoresEmpty()
    {
        MyPassword entry = new() { Data = null!, UserName = null!, Password = null!, Keyword = null! };

        Assert.AreEqual("", entry.Data);
        Assert.AreEqual("", entry.UserName);
        Assert.AreEqual("", entry.Password);
        Assert.AreEqual("", entry.Keyword);
        Assert.AreEqual("", entry.Title);
    }
}
