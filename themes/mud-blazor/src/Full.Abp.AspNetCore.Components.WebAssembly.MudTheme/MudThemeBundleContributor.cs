using Volo.Abp.Bundling;

namespace Full.Abp.AspNetCore.Components.WebAssembly.MudTheme;

public class MudThemeBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {
        context.Add("_content/MudBlazor/MudBlazor.min.js");
    }

    public void AddStyles(BundleContext context)
    {
        context.Add("_content/Full.Abp.AspNetCore.Components.Web.MudTheme/libs/abp/css/theme.css");

        context.Add("https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap");
        context.Add("_content/MudBlazor/MudBlazor.min.css");
    }
}