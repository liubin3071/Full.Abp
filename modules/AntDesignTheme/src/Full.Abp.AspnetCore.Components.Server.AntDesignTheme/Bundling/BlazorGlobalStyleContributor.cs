using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Full.Abp.AspnetCore.Components.Server.AntDesignTheme.Bundling;

public class BlazorGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/AntDesign/css/ant-design-blazor.css");
    }
}
