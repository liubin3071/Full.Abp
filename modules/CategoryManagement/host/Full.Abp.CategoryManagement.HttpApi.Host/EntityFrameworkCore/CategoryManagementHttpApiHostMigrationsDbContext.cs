using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Full.Abp.CategoryManagement.EntityFrameworkCore;

public class CategoryManagementHttpApiHostMigrationsDbContext : AbpDbContext<CategoryManagementHttpApiHostMigrationsDbContext>
{
    public CategoryManagementHttpApiHostMigrationsDbContext(DbContextOptions<CategoryManagementHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureCategoryManagement();
    }
}
