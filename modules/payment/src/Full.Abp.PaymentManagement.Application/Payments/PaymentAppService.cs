using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Full.Abp.PaymentManagement.Payments;

public class PaymentAppService : CrudAppService<Payment, PaymentGetOutput, PaymentGetListOutput, Guid,
    PaymentGetListInput, PaymentCreateInput,
    PaymentUpdateInput>, IPaymentAppService
{
    public PaymentAppService(IRepository<Payment, Guid> repository) : base(repository)
    {
    }
}