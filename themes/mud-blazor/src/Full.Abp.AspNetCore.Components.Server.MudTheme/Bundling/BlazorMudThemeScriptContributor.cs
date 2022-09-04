using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Full.Abp.AspNetCore.Components.Server.MudTheme.Bundling;

public class BlazorMudThemeScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/MudBlazor/MudBlazor.min.js");
        base.ConfigureBundle(context);
    }
}