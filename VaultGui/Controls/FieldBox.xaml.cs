using System.Windows.Input;
using VaultGui.Resources.Strings;

namespace VaultGui.Controls;

/// <summary>
/// Text field in a card: the one look of every input of the app.
/// With <see cref="IsPassword"/> it gets an eye button to show or hide what is typed (see <see cref="PasswordBox"/>).
/// </summary>
public partial class FieldBox : ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(FieldBox), "", BindingMode.TwoWay,
        propertyChanged: (bindable, _, value) =>
        {
            // A password field that is emptied hides again: the pages clear it after use,
            // so a later password is never typed in clear by surprise.
            if (string.IsNullOrEmpty((string?)value))
                ((FieldBox)bindable).IsPasswordHidden = true;
        });

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder), typeof(string), typeof(FieldBox), "");

    public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
        nameof(Keyboard), typeof(Keyboard), typeof(FieldBox), Keyboard.Default);

    public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
        nameof(ReturnType), typeof(ReturnType), typeof(FieldBox), ReturnType.Default);

    public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
        nameof(ReturnCommand), typeof(ICommand), typeof(FieldBox));

    public static readonly BindableProperty IsSpellCheckEnabledProperty = BindableProperty.Create(
        nameof(IsSpellCheckEnabled), typeof(bool), typeof(FieldBox), true);

    public static readonly BindableProperty IsTextPredictionEnabledProperty = BindableProperty.Create(
        nameof(IsTextPredictionEnabled), typeof(bool), typeof(FieldBox), true);

    public static readonly BindableProperty ClearButtonVisibilityProperty = BindableProperty.Create(
        nameof(ClearButtonVisibility), typeof(ClearButtonVisibility), typeof(FieldBox), ClearButtonVisibility.Never);

    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(ImageSource), typeof(FieldBox),
        propertyChanged: (bindable, _, value) =>
        {
            FieldBox box = (FieldBox)bindable;
            box.LeadingIcon.Source = (ImageSource?)value;
            box.LeadingIcon.IsVisible = value is not null;
        });

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        nameof(IsPassword), typeof(bool), typeof(FieldBox), false,
        propertyChanged: (bindable, _, _) => ((FieldBox)bindable).UpdatePassword());

    public static readonly BindableProperty IsPasswordHiddenProperty = BindableProperty.Create(
        nameof(IsPasswordHidden), typeof(bool), typeof(FieldBox), true,
        propertyChanged: (bindable, _, _) => ((FieldBox)bindable).UpdatePassword());

    public FieldBox()
    {
        InitializeComponent();
        Input.HandlerChanged += (_, _) => RemoveNativeFrame();
        UpdatePassword();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    public ReturnType ReturnType
    {
        get => (ReturnType)GetValue(ReturnTypeProperty);
        set => SetValue(ReturnTypeProperty, value);
    }

    public ICommand? ReturnCommand
    {
        get => (ICommand?)GetValue(ReturnCommandProperty);
        set => SetValue(ReturnCommandProperty, value);
    }

    public bool IsSpellCheckEnabled
    {
        get => (bool)GetValue(IsSpellCheckEnabledProperty);
        set => SetValue(IsSpellCheckEnabledProperty, value);
    }

    public bool IsTextPredictionEnabled
    {
        get => (bool)GetValue(IsTextPredictionEnabledProperty);
        set => SetValue(IsTextPredictionEnabledProperty, value);
    }

    /// <summary>Cross at the end of the field to empty it.</summary>
    public ClearButtonVisibility ClearButtonVisibility
    {
        get => (ClearButtonVisibility)GetValue(ClearButtonVisibilityProperty);
        set => SetValue(ClearButtonVisibilityProperty, value);
    }

    /// <summary>Small image before the text (the magnifier of a search field).</summary>
    public ImageSource? Icon
    {
        get => (ImageSource?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>Password field: typed text is masked and the eye button is shown.</summary>
    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    /// <summary>State of the eye (password fields only): true = masked.</summary>
    public bool IsPasswordHidden
    {
        get => (bool)GetValue(IsPasswordHiddenProperty);
        set => SetValue(IsPasswordHiddenProperty, value);
    }

    /// <summary>Put the caret in the field.</summary>
    public bool FocusInput() => Input.Focus();

    private void OnEyeClicked(object? sender, EventArgs e) => IsPasswordHidden = !IsPasswordHidden;

    private void UpdatePassword()
    {
        Input.IsPassword = IsPassword && IsPasswordHidden;
        Eye.IsVisible = IsPassword;

        string text = Loc.Get(IsPasswordHidden ? Loc.Show : Loc.Hide);
        // Purple line on light, pale line on dark.
        Eye.SetAppTheme<ImageSource>(ImageButton.SourceProperty,
            IsPasswordHidden ? "eye.png" : "eye_off.png",
            IsPasswordHidden ? "eye_dark.png" : "eye_off_dark.png");
        ToolTipProperties.SetText(Eye, text);
        SemanticProperties.SetDescription(Eye, text);
    }

    /// <summary>The native text box draws its own frame or underline: the card replaces it.</summary>
    private void RemoveNativeFrame()
    {
#if WINDOWS
        if (Input.Handler?.PlatformView is not Microsoft.UI.Xaml.Controls.TextBox box)
            return;

        Microsoft.UI.Xaml.Thickness none = new(0);
        Microsoft.UI.Xaml.Media.SolidColorBrush clear = new(Microsoft.UI.Colors.Transparent);

        // The focused state of the WinUI template takes these from resources, not from the properties.
        box.BorderThickness = none;
        box.Resources["TextControlBorderThemeThickness"] = none;
        box.Resources["TextControlBorderThemeThicknessFocused"] = none;
        box.Resources["TextControlBackground"] = clear;
        box.Resources["TextControlBackgroundPointerOver"] = clear;
        box.Resources["TextControlBackgroundFocused"] = clear;
        box.Resources["TextControlBackgroundDisabled"] = clear;
#elif ANDROID
        // Hides the underline and keeps the default padding.
        if (Input.Handler?.PlatformView is Android.Widget.EditText text)
            text.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
    }
}
