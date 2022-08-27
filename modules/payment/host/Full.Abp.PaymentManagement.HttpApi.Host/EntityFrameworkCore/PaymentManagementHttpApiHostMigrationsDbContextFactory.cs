using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Full.Abp.PaymentManagement.EntityFrameworkCore;

public class PaymentManagementHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<PaymentManagementHttpApiHostMigrationsDbContext>
{
    public PaymentManagementHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<PaymentManagementHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("PaymentManagement"));

        return new PaymentManagementHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
