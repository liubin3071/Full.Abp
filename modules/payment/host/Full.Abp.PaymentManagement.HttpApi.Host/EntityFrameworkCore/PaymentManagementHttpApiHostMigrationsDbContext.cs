using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Full.Abp.PaymentManagement.EntityFrameworkCore;

public class PaymentManagementHttpApiHostMigrationsDbContext : AbpDbContext<PaymentManagementHttpApiHostMigrationsDbContext>
{
    public PaymentManagementHttpApiHostMigrationsDbContext(DbContextOptions<PaymentManagementHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigurePaymentManagement();
    }
}
