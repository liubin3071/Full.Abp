using Full.Abp.PaymentManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Full.Abp.PaymentManagement.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class PaymentManagementPageModel : AbpPageModel
{
    protected PaymentManagementPageModel()
    {
        LocalizationResourceType = typeof(PaymentManagementResource);
        ObjectMapperContext = typeof(PaymentManagementWebModule);
    }
}
