using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Full.Abp.FinancialManagement.EntityFrameworkCore;

[DependsOn(
    typeof(FinancialManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class FinancialManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<FinancialManagementDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddDefaultRepositories(true);
        });
    }
}