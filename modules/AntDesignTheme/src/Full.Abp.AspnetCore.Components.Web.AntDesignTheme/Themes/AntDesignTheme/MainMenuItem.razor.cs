using AntDesign;
using Microsoft.AspNetCore.Components;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.AspnetCore.Components.Web.AntDesignTheme.Themes.AntDesignTheme;

public partial class MainMenuItem : ComponentBase
{
    [Parameter]
    public ApplicationMenuItem Menu { get; set; }
}
