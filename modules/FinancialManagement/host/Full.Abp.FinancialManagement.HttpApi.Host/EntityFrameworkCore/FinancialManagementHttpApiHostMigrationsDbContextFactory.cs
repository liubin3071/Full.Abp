using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Full.Abp.FinancialManagement.EntityFrameworkCore;

public class FinancialManagementHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<FinancialManagementHttpApiHostMigrationsDbContext>
{
    public FinancialManagementHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<FinancialManagementHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("FinancialManagement"));

        return new FinancialManagementHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
