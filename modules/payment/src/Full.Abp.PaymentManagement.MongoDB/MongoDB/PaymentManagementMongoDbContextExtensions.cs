using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Full.Abp.PaymentManagement.MongoDB;

public static class PaymentManagementMongoDbContextExtensions
{
    public static void ConfigurePaymentManagement(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
