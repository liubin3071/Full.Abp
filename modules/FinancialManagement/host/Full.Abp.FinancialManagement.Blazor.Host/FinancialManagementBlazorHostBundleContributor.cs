using Volo.Abp.Bundling;

namespace Full.Abp.FinancialManagement.Blazor.Host;

public class FinancialManagementBlazorHostBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {

    }

    public void AddStyles(BundleContext context)
    {
        context.Add("main.css", true);
    }
}
