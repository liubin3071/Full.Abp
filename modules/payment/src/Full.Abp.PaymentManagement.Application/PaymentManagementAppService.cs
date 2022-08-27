using Full.Abp.PaymentManagement.Localization;
using Volo.Abp.Application.Services;

namespace Full.Abp.PaymentManagement;

public abstract class PaymentManagementAppService : ApplicationService
{
    protected PaymentManagementAppService()
    {
        LocalizationResource = typeof(PaymentManagementResource);
        ObjectMapperContext = typeof(PaymentManagementApplicationModule);
    }
}
