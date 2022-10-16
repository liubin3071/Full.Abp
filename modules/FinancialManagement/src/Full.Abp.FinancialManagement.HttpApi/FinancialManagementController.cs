using Full.Abp.FinancialManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Full.Abp.FinancialManagement;

public abstract class FinancialManagementController : AbpControllerBase
{
    protected FinancialManagementController()
    {
        LocalizationResource = typeof(FinancialManagementResource);
    }
}
