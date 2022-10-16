using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Full.Abp.FinancialManagement.Blazor.Server.Host;

[Dependency(ReplaceServices = true)]
public class FinancialManagementBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "FinancialManagement";
}
