using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Full.Abp.PaymentManagement;

[Dependency(ReplaceServices = true)]
public class PaymentManagementBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "PaymentManagement";
}
