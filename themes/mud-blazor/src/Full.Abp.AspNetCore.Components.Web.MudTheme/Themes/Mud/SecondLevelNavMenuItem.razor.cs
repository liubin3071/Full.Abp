using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.AspNetCore.Components.Web.MudTheme.Themes.Mud;

public partial class SecondLevelNavMenuItem
{
    [Inject] private NavigationManager NavigationManager { get; set; }

    [Parameter] public ApplicationMenuItem MenuItem { get; set; }

    [Parameter] public EventCallback OnActive { get; set; }
    public bool IsSubMenuOpen { get; set; }

    protected override Task OnInitializedAsync()
    {
        if (!MenuItem.IsLeaf)
        {
            return base.OnInitializedAsync();
        }

        var link = NavigationManager.ToAbsoluteUri(MenuItem.Url.TrimStart('/', '~'));
        if (NavigationManager.Uri.StartsWith(link.ToString()))
        {
            OnActive.InvokeAsync();
        }

        return base.OnInitializedAsync();
    }

    private async Task OnChildrenActive()
    {
        IsSubMenuOpen = true;
        await OnActive.InvokeAsync();
    }
}