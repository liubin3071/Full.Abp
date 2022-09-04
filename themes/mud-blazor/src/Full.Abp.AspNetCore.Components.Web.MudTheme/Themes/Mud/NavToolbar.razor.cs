using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;

namespace Full.Abp.AspNetCore.Components.Web.MudTheme.Themes.Mud;

public partial class NavToolbar : IDisposable
{
    [Inject]
    private IToolbarManager ToolbarManager { get; set; }

    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private List<RenderFragment> ToolbarItemRenders { get; set; } = new List<RenderFragment>();

    protected override async Task OnInitializedAsync()
    {
        await GetToolbarItemRendersAsync();
        AuthenticationStateProvider.AuthenticationStateChanged += AuthenticationStateProviderOnAuthenticationStateChanged;
    }

    private async Task GetToolbarItemRendersAsync()
    {
        var toolbar = await ToolbarManager.GetAsync(StandardToolbars.Main);

        ToolbarItemRenders.Clear();

        var sequence = 0;
        foreach (var item in toolbar.Items)
        {
            ToolbarItemRenders.Add(builder =>
            {
                builder.OpenComponent(sequence++, item.ComponentType);
                builder.CloseComponent();
            });
        }
    }

    private async void AuthenticationStateProviderOnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        await GetToolbarItemRendersAsync();
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        AuthenticationStateProvider.AuthenticationStateChanged -= AuthenticationStateProviderOnAuthenticationStateChanged;
    }
}
