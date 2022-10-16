using Full.Abp.FinancialManagement.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Full.Abp.FinancialManagement.Blazor.Server.Host;

public abstract class FinancialManagementComponentBase : AbpComponentBase
{
    protected FinancialManagementComponentBase()
    {
        LocalizationResource = typeof(FinancialManagementResource);
    }
}
