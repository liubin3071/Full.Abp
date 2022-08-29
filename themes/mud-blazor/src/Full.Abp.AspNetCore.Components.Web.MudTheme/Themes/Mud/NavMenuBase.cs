using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.AspNetCore.Components.Web.MudTheme.Themes.Mud;

public abstract class NavMenuBase :AbpComponentBase, IDisposable
{
    public abstract string MenuName { get; }
    
    [Inject]
    protected IMenuManager MenuManager { get; set; }

    [Inject]
    protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    protected ApplicationMenu? Menu { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Menu = await MenuManager.GetAsync(MenuName);
        AuthenticationStateProvider.AuthenticationStateChanged += AuthenticationStateProviderOnAuthenticationStateChanged;
    }

    public void Dispose()
    {
        AuthenticationStateProvider.AuthenticationStateChanged -= AuthenticationStateProviderOnAuthenticationStateChanged;
    }

    private async void AuthenticationStateProviderOnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        Menu = await MenuManager.GetMainMenuAsync();
        await InvokeAsync(StateHasChanged);
    }
}
