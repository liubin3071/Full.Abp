using Full.Abp.PaymentManagement.Payments;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Full.Abp.PaymentManagement.EntityFrameworkCore;

[ConnectionStringName(PaymentManagementDbProperties.ConnectionStringName)]
public interface IPaymentManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
    DbSet<Payment> Payments { get; }
    DbSet<Refund> Refunds { get; }
    DbSet<PaymentGateway> PaymentGateways { get; }
    DbSet<PaymentChannel> PaymentChannels { get; }
}
