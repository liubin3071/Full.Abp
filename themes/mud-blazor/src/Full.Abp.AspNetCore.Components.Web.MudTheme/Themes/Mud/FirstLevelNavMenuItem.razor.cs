using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.AspNetCore.Components.Web.MudTheme.Themes.Mud;

public partial class FirstLevelNavMenuItem
{
    [Parameter] public ApplicationMenuItem MenuItem { get; set; }
    public bool IsSubMenuOpen { get; set; }
 
    private Task OnChildrenActive()
    {
        IsSubMenuOpen = true;
        return Task.CompletedTask;
    }
}