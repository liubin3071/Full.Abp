using Volo.Abp.Bundling;

namespace Full.Abp.CategoryManagement.Blazor.Host;

public class CategoryManagementBlazorHostBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {

    }

    public void AddStyles(BundleContext context)
    {
        context.Add("main.css", true);
    }
}
