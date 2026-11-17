namespace VaultCore;

public enum PasswordStrengthLevel
{
    /// <summary>Nothing typed.</summary>
    None,
    VeryWeak,
    Weak,
    Fair,
    Strong,
    VeryStrong,
}

/// <summary>
/// Estimated strength of a master password.
/// </summary>
/// <param name="Level">Rating shown to the user.</param>
/// <param name="Bits">Estimated entropy. An upper bound: a password a person made up is easier to guess.</param>
public readonly record struct PasswordStrength(PasswordStrengthLevel Level, double Bits)
{
    /// <summary>0 (nothing) to 1 (very strong), for a progress bar.</summary>
    public double Score => (int)Level / (double)(int)PasswordStrengthLevel.VeryStrong;
}

/// <summary>
/// Rough, offline estimate of how hard a master password is to guess. No dictionary is shipped:
/// it rewards length and variety, and penalizes repeats, keyboard or alphabet runs and a few very common words.
/// </summary>
public static class PasswordStrengthEstimator
{
    private const double WeakBits = 28;
    private const double FairBits = 40;
    private const double StrongBits = 60;
    private const double VeryStrongBits = 80;

    /// <summary>What a common word is worth, however long it is.</summary>
    private const double CommonWordBits = 8;

    private static readonly string[] CommonWords =
    [
        "password", "motdepasse", "passe", "azerty", "qwerty", "qwertz", "letmein", "welcome", "bienvenue",
        "admin", "administrator", "login", "master", "secret", "iloveyou", "jetaime", "bonjour", "soleil",
        "doudou", "loulou", "chouchou", "dragon", "monkey", "football", "princess", "princesse", "sunshine",
        "superman", "batman", "starwars", "vault", "coffre", "abc", "test",
    ];

    private static readonly string[] Runs =
    [
        "abcdefghijklmnopqrstuvwxyz", "0123456789", "azertyuiop", "qsdfghjklm", "wxcvbn",
        "qwertyuiop", "asdfghjkl", "zxcvbnm",
    ];

    public static PasswordStrength Estimate(string? password)
    {
        if (string.IsNullOrEmpty(password))
            return new PasswordStrength(PasswordStrengthLevel.None, 0);

        double bits = EstimateBits(password);
        PasswordStrengthLevel level = bits switch
        {
            < WeakBits => PasswordStrengthLevel.VeryWeak,
            < FairBits => PasswordStrengthLevel.Weak,
            < StrongBits => PasswordStrengthLevel.Fair,
            < VeryStrongBits => PasswordStrengthLevel.Strong,
            _ => PasswordStrengthLevel.VeryStrong,
        };
        return new PasswordStrength(level, bits);
    }

    private static double EstimateBits(string password)
    {
        string normalized = Normalize(password);

        // A common word (even written "P@ssw0rd") counts as a single cheap guess.
        double wordBits = 0;
        string? word = CommonWords.Where(normalized.Contains).MaxBy(x => x.Length);
        if (word is not null)
        {
            int at = normalized.IndexOf(word, StringComparison.Ordinal);
            password = password.Remove(at, word.Length);
            wordBits = CommonWordBits;
        }

        // What is left around the word is rated on its own ("2026" after it is four digits, no more).
        double bitsPerChar = Math.Log2(PoolSize(password));

        // Each character counts fully, unless it only repeats or continues what precedes it.
        double length = 0;
        for (int i = 0; i < password.Length; i++)
        {
            bool predictable = i > 0 && IsPredictable(char.ToLowerInvariant(password[i - 1]), char.ToLowerInvariant(password[i]));
            length += predictable ? 0.25 : 1;
        }

        // "abababab" is long but made of two characters.
        int distinct = password.Distinct().Count();
        double bits = Math.Min(length * bitsPerChar, distinct * bitsPerChar * 2);
        return bits + wordBits;
    }

    /// <summary>Number of characters the password seems to be drawn from.</summary>
    private static int PoolSize(string password)
    {
        int pool = 0;
        if (password.Any(char.IsAsciiLetterLower)) pool += 26;
        if (password.Any(char.IsAsciiLetterUpper)) pool += 26;
        if (password.Any(char.IsAsciiDigit)) pool += 10;
        if (password.Any(c => c < 128 && !char.IsAsciiLetterOrDigit(c))) pool += 33;
        if (password.Any(c => c >= 128)) pool += 50;
        return Math.Max(pool, 2);
    }

    /// <summary>Same character, or neighbours in the alphabet, the digits or a keyboard row (either direction).</summary>
    private static bool IsPredictable(char previous, char current)
    {
        if (previous == current)
            return true;

        foreach (string run in Runs)
        {
            int a = run.IndexOf(previous);
            int b = run.IndexOf(current);
            if (a >= 0 && b >= 0 && Math.Abs(a - b) == 1)
                return true;
        }
        return false;
    }

    /// <summary>Lower case, with the usual letter look-alikes undone ("P@ssw0rd" becomes "password"). Same length.</summary>
    private static string Normalize(string password) =>
        string.Create(password.Length, password, static (span, source) =>
        {
            for (int i = 0; i < source.Length; i++)
            {
                span[i] = char.ToLowerInvariant(source[i]) switch
                {
                    '0' => 'o',
                    '1' or '!' => 'i',
                    '3' => 'e',
                    '4' or '@' => 'a',
                    '5' or '$' => 's',
                    '7' => 't',
                    char c => c,
                };
            }
        });
}
