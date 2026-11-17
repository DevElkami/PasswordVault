namespace VaultGui.Controls;

/// <summary>
/// <see cref="FieldBox"/> for a password: masked, with the eye button, no spell check or text prediction.
/// Always starts masked.
/// </summary>
public sealed class PasswordBox : FieldBox
{
    public PasswordBox()
    {
        IsPassword = true;
        IsSpellCheckEnabled = false;
        IsTextPredictionEnabled = false;
    }
}
