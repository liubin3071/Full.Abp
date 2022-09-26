using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Full.Abp.AspnetCore.Components.Server.AntDesignTheme.Bundling;

public class BlazorAntDesignThemeStyleContributor: BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/Full.Abp.AspnetCore.Components.Web.AntDesignTheme/libs/abp/css/theme.css");
    }
}
