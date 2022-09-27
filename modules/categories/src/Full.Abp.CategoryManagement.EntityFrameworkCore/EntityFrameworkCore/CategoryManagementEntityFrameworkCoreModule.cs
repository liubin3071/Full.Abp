using Full.Abp.Categories;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Full.Abp.CategoryManagement.EntityFrameworkCore;

[DependsOn(
    typeof(CategoryManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class CategoryManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CategoryManagementDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<Category, CategoryRepository>();
                options.AddDefaultRepositories();
        });
    }
}
