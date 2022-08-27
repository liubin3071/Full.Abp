using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Full.Abp.PaymentManagement.EntityFrameworkCore;

[ConnectionStringName(PaymentManagementDbProperties.ConnectionStringName)]
public interface IPaymentManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
