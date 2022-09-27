using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic;

public partial class NavMenu : IDisposable
{
    [Inject]
    protected IMenuManager MenuManager { get; set; }

    [Inject]
    protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    protected ApplicationMenu Menu { get; set; }

    protected async override Task OnInitializedAsync()
    {
        Menu = await MenuManager.GetMainMenuAsync();
        AuthenticationStateProvider.AuthenticationStateChanged +=
            AuthenticationStateProviderOnAuthenticationStateChanged;
    }

    public void Dispose()
    {
        AuthenticationStateProvider.AuthenticationStateChanged -=
            AuthenticationStateProviderOnAuthenticationStateChanged;
    }

    private async void AuthenticationStateProviderOnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        Menu = await MenuManager.GetMainMenuAsync();
        await InvokeAsync(StateHasChanged);
    }
}