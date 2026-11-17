using VaultCore;
using VaultCore.Models;

namespace VaultTestProject;

[TestClass]
public class UnitTestDuplicates
{
    private static MyPassword Entry(string site, string user = "me", string keyword = "") =>
        new() { Data = site, UserName = user, Password = "p", Keyword = keyword };

    [TestMethod]
    [DataRow("https://www.Example.com/login?x=1", "example.com")]
    [DataRow("example.com", "example.com")]
    [DataRow("HTTP://example.com:8443/", "example.com")]
    [DataRow("www.example.com#top", "example.com")]
    [DataRow("  Un site web ", "un site web")]
    [DataRow("Banque / compte joint", "banque / compte joint")]
    public void SiteKeySimplifiesAddresses(string site, string expected)
    {
        Assert.AreEqual(expected, VaultDuplicates.SiteKey(Entry(site)));
    }

    [TestMethod]
    public void SiteKeyUsesKeywordWhenNoSite()
    {
        Assert.AreEqual("wifi", VaultDuplicates.SiteKey(Entry("", keyword: "WiFi")));
        Assert.AreEqual("", VaultDuplicates.SiteKey(Entry("")));
    }

    [TestMethod]
    public void FindGroupsEntriesOfTheSameSite()
    {
        MyPassword a = Entry("https://www.example.com/login", "zoe");
        MyPassword b = Entry("example.com", "adam");
        MyPassword c = Entry("other.org");
        MyPassword d = Entry("", keyword: "wifi");
        MyPassword e = Entry("", keyword: "WIFI", user: "guest");
        MyPassword empty1 = Entry("");
        MyPassword empty2 = Entry("");

        IReadOnlyList<IReadOnlyList<MyPassword>> groups = VaultDuplicates.Find([a, b, c, d, e, empty1, empty2]);

        Assert.AreEqual(2, groups.Count);
        CollectionAssert.AreEqual(new[] { b, a }, groups[0].ToArray()); // example.com, ordered by login
        CollectionAssert.AreEqual(new[] { e, d }, groups[1].ToArray()); // wifi
    }

    [TestMethod]
    public void FindReturnsNothingWithoutDuplicates()
    {
        Assert.AreEqual(0, VaultDuplicates.Find([Entry("a.com"), Entry("b.com")]).Count);
    }
}
