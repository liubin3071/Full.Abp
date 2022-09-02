using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Full.Abp.AspNetCore.Components.Server.BasicTheme.Bundling;

public class BlazorBasicThemeStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains(
            "/_content/Full.Abp.AspNetCore.Components.Web.BasicTheme/libs/abp/css/theme.css");
    }
}