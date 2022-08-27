using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Full.Abp.PaymentManagement.MongoDB;

[ConnectionStringName(PaymentManagementDbProperties.ConnectionStringName)]
public class PaymentManagementMongoDbContext : AbpMongoDbContext, IPaymentManagementMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigurePaymentManagement();
    }
}
