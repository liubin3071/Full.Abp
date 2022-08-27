using Full.Abp.PaymentManagement.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Full.Abp.PaymentManagement.Blazor.Server.Host;

public abstract class PaymentManagementComponentBase : AbpComponentBase
{
    protected PaymentManagementComponentBase()
    {
        LocalizationResource = typeof(PaymentManagementResource);
    }
}
