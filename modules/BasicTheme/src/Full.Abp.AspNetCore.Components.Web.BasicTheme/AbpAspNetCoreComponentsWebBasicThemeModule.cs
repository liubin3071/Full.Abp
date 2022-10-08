using Full.Abp.BlazoriseUI;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.Modularity;

namespace Full.Abp.AspNetCore.Components.Web.BasicTheme;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpBlazoriseUIModule)
)]
public class AbpAspNetCoreComponentsWebBasicThemeModule : AbpModule
{
}