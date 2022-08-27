using Volo.Abp.Bundling;

namespace Full.Abp.PaymentManagement.Blazor.Host;

public class PaymentManagementBlazorHostBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {

    }

    public void AddStyles(BundleContext context)
    {
        context.Add("main.css", true);
    }
}
