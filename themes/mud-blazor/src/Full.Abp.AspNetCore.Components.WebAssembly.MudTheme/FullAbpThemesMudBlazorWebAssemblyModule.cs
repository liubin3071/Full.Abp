using Full.Abp.AspNetCore.Components.Web.MudTheme;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Http.Client.IdentityModel.WebAssembly;
using Volo.Abp.Modularity;

namespace Full.Abp.AspNetCore.Components.WebAssembly.MudTheme;

[DependsOn(
    typeof(FullAbpThemesMudBlazorModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
    typeof(AbpHttpClientIdentityModelWebAssemblyModule)
)]
public class FullAbpThemesMudBlazorWebAssemblyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(FullAbpThemesMudBlazorWebAssemblyModule).Assembly);
        });

        Configure<AbpToolbarOptions>(options => { options.Contributors.Add(new MudThemeToolbarContributor()); });
    }
}