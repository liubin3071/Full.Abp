using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Full.Abp.FinancialManagement.EntityFrameworkCore;

public class FinancialManagementHttpApiHostMigrationsDbContext : AbpDbContext<FinancialManagementHttpApiHostMigrationsDbContext>
{
    public FinancialManagementHttpApiHostMigrationsDbContext(DbContextOptions<FinancialManagementHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureFinancialManagement();
    }
}
