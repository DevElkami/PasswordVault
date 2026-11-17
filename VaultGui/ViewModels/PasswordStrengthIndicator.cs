using CommunityToolkit.Mvvm.ComponentModel;
using VaultGui.Resources.Strings;
using VaultCore;

namespace VaultGui.ViewModels;

/// <summary>
/// Strength of the master password being typed (bar + text), for the pages where one is chosen.
/// </summary>
public partial class PasswordStrengthIndicator : ObservableObject
{
    private static readonly Color WeakColor = Color.FromArgb("#D64545");
    private static readonly Color FairColor = Color.FromArgb("#D9822B");
    private static readonly Color StrongColor = Color.FromArgb("#2E9E5B");

    /// <summary>Something is typed: show the indicator.</summary>
    [ObservableProperty]
    public partial bool IsVisible { get; set; }

    /// <summary>0 to 1.</summary>
    [ObservableProperty]
    public partial double Score { get; set; }

    [ObservableProperty]
    public partial string Text { get; set; } = "";

    [ObservableProperty]
    public partial Color Color { get; set; } = WeakColor;

    public void Update(string password)
    {
        PasswordStrength strength = PasswordStrengthEstimator.Estimate(password);
        IsVisible = strength.Level != PasswordStrengthLevel.None;
        Score = strength.Score;
        Color = strength.Level switch
        {
            >= PasswordStrengthLevel.Strong => StrongColor,
            PasswordStrengthLevel.Fair => FairColor,
            _ => WeakColor,
        };
        Text = IsVisible
            ? Loc.Format(Loc.PasswordStrength, Loc.Get(strength.Level switch
            {
                PasswordStrengthLevel.VeryWeak => Loc.StrengthVeryWeak,
                PasswordStrengthLevel.Weak => Loc.StrengthWeak,
                PasswordStrengthLevel.Fair => Loc.StrengthFair,
                PasswordStrengthLevel.Strong => Loc.StrengthStrong,
                _ => Loc.StrengthVeryStrong,
            }))
            : "";
    }
}
