using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Full.Abp.PaymentManagement.Blazor.Server.Host;

[Dependency(ReplaceServices = true)]
public class PaymentManagementBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "PaymentManagement";
}
