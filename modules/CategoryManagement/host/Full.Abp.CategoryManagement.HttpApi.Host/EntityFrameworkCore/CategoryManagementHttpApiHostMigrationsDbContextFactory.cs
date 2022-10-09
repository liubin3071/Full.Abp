using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Full.Abp.CategoryManagement.EntityFrameworkCore;

public class CategoryManagementHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<CategoryManagementHttpApiHostMigrationsDbContext>
{
    public CategoryManagementHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<CategoryManagementHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("CategoryManagement"));

        return new CategoryManagementHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
