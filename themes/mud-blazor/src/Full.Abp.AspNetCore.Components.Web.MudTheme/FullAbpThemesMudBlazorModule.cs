using MudBlazor.Services;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.Modularity;

namespace Full.Abp.AspNetCore.Components.Web.MudTheme;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule)
)]
public class FullAbpThemesMudBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
        context.Services.AddMudServices();
    }
}