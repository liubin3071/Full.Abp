using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Full.Abp.PaymentManagement.Payments;

public interface IPaymentAppService :
    ICrudAppService<PaymentGetOutput, PaymentGetListOutput, Guid, PaymentGetListInput, PaymentCreateInput,
        PaymentUpdateInput>, IApplicationService
{
}

public class PaymentUpdateInput
{
}

public class PaymentCreateInput
{
}

public class PaymentGetListInput
{
}

public class PaymentGetListOutput : IEntityDto<Guid>
{
    public Guid Id { get; set; }
}

public class PaymentGetOutput : IEntityDto<Guid>
{
    public Guid Id { get; set; }
}