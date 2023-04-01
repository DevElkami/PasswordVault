using System.Diagnostics;

namespace MauiPasswordVault.Service;

public class NavigationService
{
    readonly IServiceProvider services;

    public NavigationService(IServiceProvider services)
    {
        this.services = services;
    }

    public Task NavigateToPage<T>() where T : Page
    {
        var page = ResolvePage<T>();
        if (page is not null)
            return Navigation.PushAsync(page, true);
        throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
    }

    public Task NavigateBack()
    {
        if (Navigation.NavigationStack.Count > 1)
            return Navigation.PopAsync();
        throw new InvalidOperationException("No pages to navigate back to!");
    }

    protected INavigation Navigation
    {
        get
        {
            INavigation? navigation = Application.Current?.MainPage?.Navigation;
            if (navigation is not null)
                return navigation;
            else
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                throw new Exception("Can't get navigation.");
            }
        }
    }

    private T? ResolvePage<T>() where T : Page
    {
        return services.GetService<T>();
    }
}
