using Full.Abp.PaymentManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Full.Abp.PaymentManagement;

public abstract class PaymentManagementController : AbpControllerBase
{
    protected PaymentManagementController()
    {
        LocalizationResource = typeof(PaymentManagementResource);
    }
}
