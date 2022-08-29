using Full.Abp.AspNetCore.Components.Server.MudTheme.Bundling;
using Full.Abp.AspNetCore.Components.Web.MudTheme;
using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.AspNetCore.Components.Server.Theming.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Full.Abp.AspNetCore.Components.Server.MudTheme;

[DependsOn(
    typeof(FullAbpThemesMudBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule)
)]
public class FullAbpThemesMudBlazorServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpToolbarOptions>(options => { options.Contributors.Add(new MudThemeToolbarContributor()); });

        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(BlazorMudThemeBundles.Styles.Global, bundle =>
                {
                    bundle
                        .AddBaseBundles(BlazorStandardBundles.Styles.Global)
                        .AddContributors(typeof(BlazorMudThemeStyleContributor));
                });

            options
                .ScriptBundles
                .Add(BlazorMudThemeBundles.Scripts.Global, bundle =>
                {
                    bundle
                        .AddBaseBundles(BlazorStandardBundles.Scripts.Global)
                        .AddContributors(typeof(BlazorMudThemeScriptContributor));
                });
        });
    }
}