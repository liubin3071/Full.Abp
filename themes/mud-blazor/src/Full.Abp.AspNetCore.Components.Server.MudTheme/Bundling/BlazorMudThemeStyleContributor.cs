using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Full.Abp.AspNetCore.Components.Server.MudTheme.Bundling;

public class BlazorMudThemeStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        // context.Files.AddIfNotContains(
        //     "/_content/Full.Abp.AspNetCore.Components.Web.MudTheme/libs/abp/css/theme.css")
        //     ;
        context.Files.AddIfNotContains("/_content/MudBlazor/MudBlazor.min.css");
    }
}