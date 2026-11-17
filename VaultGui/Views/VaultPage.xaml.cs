using System.Windows.Input;
using VaultGui.Resources.Strings;
using VaultGui.ViewModels;

namespace VaultGui.Views;

public partial class VaultPage : ContentPage
{
    private readonly VaultViewModel viewModel;

    public VaultPage(VaultViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;

#if WINDOWS
        // Windows: no header band, the actions are icons in the title bar.
        Shell.SetNavBarIsVisible(this, false);
        ToolbarItems.Clear();
        BuildTitleBarActions();
#else
        // Filled icon while the list only shows duplicates.
        viewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(VaultViewModel.ShowDuplicates))
                DuplicatesItem.IconImageSource = viewModel.ShowDuplicates ? "tb_duplicates_on.png" : "tb_duplicates.png";
        };
#endif
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();

#if !WINDOWS
        // Only the actions that are set up: "Send" (to the server) and "Update" (from the https address).
        SetToolbarItem(UploadItem, viewModel.IsUploadConfigured, 0);
        SetToolbarItem(UpdateItem, viewModel.IsUpdateConfigured, ToolbarItems.Contains(UploadItem) ? 1 : 0);
#endif
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        viewModel.OnDisappearing();
    }

#if WINDOWS
    private static readonly Color ActiveAction = Color.FromRgba(255, 255, 255, 64);

    private readonly HorizontalStackLayout titleBarActions = new() { Spacing = 2, VerticalOptions = LayoutOptions.Center };
    private ImageButton duplicatesAction = null!;
    private ImageButton settingsAction = null!;
    private ImageButton uploadAction = null!;
    private ImageButton updateAction = null!;

    /// <summary>Page currently shown: the list itself, or Settings / an entry opened on top of it.</summary>
    private Page? shownPage;

    private void BuildTitleBarActions()
    {
        uploadAction = TitleBarAction("tb_upload.png", Loc.Get(Loc.UploadShort), () => RunOnListAsync(viewModel.UploadCommand));
        updateAction = TitleBarAction("tb_update.png", Loc.Get(Loc.Update), () => RunOnListAsync(viewModel.UpdateCommand));
        duplicatesAction = TitleBarAction("tb_duplicates.png", viewModel.DuplicatesText, () => RunOnListAsync(viewModel.ToggleDuplicatesCommand));
        settingsAction = TitleBarAction("tb_settings.png", Loc.Get(Loc.Settings), ToggleSettingsAsync);

        titleBarActions.Add(uploadAction);
        titleBarActions.Add(updateAction);
        titleBarActions.Add(duplicatesAction);
        titleBarActions.Add(TitleBarAction("tb_lock.png", Loc.Get(Loc.Lock), () =>
        {
            viewModel.LockCommand.Execute(null);
            return Task.CompletedTask;
        }));
        titleBarActions.Add(settingsAction);

        viewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(VaultViewModel.ShowDuplicates))
                UpdateDuplicatesAction();
        };
    }

    private static ImageButton TitleBarAction(string icon, string text, Func<Task> action)
    {
        ImageButton button = new()
        {
            Source = icon,
            Aspect = Aspect.AspectFit,
            Padding = 7,
            WidthRequest = 34,
            HeightRequest = 34,
            MinimumWidthRequest = 34,
            MinimumHeightRequest = 34,
            CornerRadius = 6,
            BackgroundColor = Colors.Transparent,
            VerticalOptions = LayoutOptions.Center,
        };
        button.Clicked += async (_, _) => await action();
        ToolTipProperties.SetText(button, text);
        SemanticProperties.SetDescription(button, text);
        return button;
    }

    /// <summary>The actions work from Settings or an entry too: that page is closed first.</summary>
    private async Task RunOnListAsync(ICommand command)
    {
        await BackToListAsync();
        if (command.CanExecute(null))
            command.Execute(null);
    }

    /// <summary>Settings opens over the list; a second click closes it.</summary>
    private async Task ToggleSettingsAsync()
    {
        bool wasOpen = shownPage is SettingsPage;
        await BackToListAsync();
        if (!wasOpen)
            viewModel.SettingsCommand.Execute(null);
    }

    private async Task BackToListAsync()
    {
        if (shownPage is not null && !ReferenceEquals(shownPage, this))
            await Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// Put the actions in the title bar (called by the shell after each navigation, while the vault is open):
    /// the same icons whatever the page, so there is no "back" header on Settings or on an entry.
    /// </summary>
    public void ShowTitleBarActions(Page current)
    {
        shownPage = current;

        // Only the actions that are set up: "Send" (to the server) and "Update" (from the https address).
        uploadAction.IsVisible = viewModel.IsUploadConfigured;
        updateAction.IsVisible = viewModel.IsUpdateConfigured;
        settingsAction.BackgroundColor = current is SettingsPage ? ActiveAction : Colors.Transparent;
        UpdateDuplicatesAction();

        if (current.Window?.TitleBar is TitleBar titleBar && !ReferenceEquals(titleBar.TrailingContent, titleBarActions))
            titleBar.TrailingContent = titleBarActions;
    }

    /// <summary>Vault locked: no action left in the title bar.</summary>
    public void HideTitleBarActions(Page current)
    {
        shownPage = null;
        if (current.Window?.TitleBar is TitleBar titleBar && ReferenceEquals(titleBar.TrailingContent, titleBarActions))
            titleBar.TrailingContent = null;
    }

    /// <summary>Highlighted while the list only shows duplicates; the tooltip says what a click does.</summary>
    private void UpdateDuplicatesAction()
    {
        duplicatesAction.BackgroundColor = viewModel.ShowDuplicates ? ActiveAction : Colors.Transparent;
        ToolTipProperties.SetText(duplicatesAction, viewModel.DuplicatesText);
        SemanticProperties.SetDescription(duplicatesAction, viewModel.DuplicatesText);
    }
#else
    private void SetToolbarItem(ToolbarItem item, bool visible, int index)
    {
        if (visible && !ToolbarItems.Contains(item))
            ToolbarItems.Insert(Math.Min(index, ToolbarItems.Count), item);
        else if (!visible)
            ToolbarItems.Remove(item);
    }
#endif
}
