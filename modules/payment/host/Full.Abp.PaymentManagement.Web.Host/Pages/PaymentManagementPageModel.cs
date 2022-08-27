using Full.Abp.PaymentManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Full.Abp.PaymentManagement.Pages;

public abstract class PaymentManagementPageModel : AbpPageModel
{
    protected PaymentManagementPageModel()
    {
        LocalizationResourceType = typeof(PaymentManagementResource);
    }
}
