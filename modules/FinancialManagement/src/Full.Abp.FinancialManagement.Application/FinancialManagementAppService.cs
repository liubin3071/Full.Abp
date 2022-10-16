using Full.Abp.FinancialManagement.Localization;
using Volo.Abp.Application.Services;

namespace Full.Abp.FinancialManagement;

public abstract class FinancialManagementAppService : ApplicationService
{
    protected FinancialManagementAppService()
    {
        LocalizationResource = typeof(FinancialManagementResource);
        ObjectMapperContext = typeof(FinancialManagementApplicationModule);
    }
}
