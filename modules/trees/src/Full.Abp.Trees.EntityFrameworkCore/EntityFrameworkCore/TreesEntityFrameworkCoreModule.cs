using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Full.Abp.Trees.EntityFrameworkCore;

[DependsOn(
    typeof(TreesDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class TreesEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // context.Services.AddAbpDbContext<TreesDbContext>(options =>
        // {
        //         /* Add custom repositories here. Example:
        //          * options.AddRepository<Question, EfCoreQuestionRepository>();
        //          */
        //         context.Services.AddTransient(typeof(ITreeRelationRepository<,>),
        //             typeof(EfCoreTreeRelationRepository<,,>));
        // });
    }
}