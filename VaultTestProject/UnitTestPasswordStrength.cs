using VaultCore;

namespace VaultTestProject;

[TestClass]
public class UnitTestPasswordStrength
{
    private static PasswordStrengthLevel Level(string password) => PasswordStrengthEstimator.Estimate(password).Level;

    [TestMethod]
    public void EmptyHasNoRating()
    {
        Assert.AreEqual(PasswordStrengthLevel.None, Level(""));
        Assert.AreEqual(PasswordStrengthLevel.None, PasswordStrengthEstimator.Estimate(null).Level);
        Assert.AreEqual(0, PasswordStrengthEstimator.Estimate("").Score);
    }

    [TestMethod]
    [DataRow("password")]
    [DataRow("P@ssw0rd")]
    [DataRow("Password1!")]
    [DataRow("motdepasse2026")]
    [DataRow("123456789012")]
    [DataRow("azertyuiop")]
    [DataRow("aaaaaaaaaaaaaaaa")]
    [DataRow("abababababababab")]
    [DataRow("abcdefghijkl")]
    public void PredictablePasswordsAreVeryWeak(string password)
    {
        Assert.AreEqual(PasswordStrengthLevel.VeryWeak, Level(password));
    }

    [TestMethod]
    public void ShortRandomPasswordIsNotStrong()
    {
        Assert.IsTrue(Level("k7#Qp2x") < PasswordStrengthLevel.Strong);
        Assert.IsTrue(Level("xkqjvmzr") < PasswordStrengthLevel.Fair);
    }

    [TestMethod]
    [DataRow("cheval pile agrafe correcte mousse")]
    [DataRow("vT9#mK2$qL7@wX4!")]
    public void LongUnpredictablePasswordsAreVeryStrong(string password)
    {
        Assert.AreEqual(PasswordStrengthLevel.VeryStrong, Level(password));
        Assert.AreEqual(1, PasswordStrengthEstimator.Estimate(password).Score);
    }

    [TestMethod]
    public void LongerIsStronger()
    {
        double previous = 0;
        foreach (string password in new[] { "tr", "trvk", "trvkpm", "trvkpmqz", "trvkpmqzhw", "trvkpmqzhwnd" })
        {
            double bits = PasswordStrengthEstimator.Estimate(password).Bits;
            Assert.IsTrue(bits > previous, password);
            previous = bits;
        }
    }

    [TestMethod]
    public void VarietyIsStronger()
    {
        Assert.IsTrue(PasswordStrengthEstimator.Estimate("trvkPMqz7!").Bits > PasswordStrengthEstimator.Estimate("trvkpmqzhw").Bits);
    }
}
