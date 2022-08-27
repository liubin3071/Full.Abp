using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Full.Abp.PaymentManagement.MongoDB;

[ConnectionStringName(PaymentManagementDbProperties.ConnectionStringName)]
public interface IPaymentManagementMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}
