using Full.Abp.Categories;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Full.Abp.CategoryManagement.Full.Abp.Categories;

[DependsOn(
    typeof(CategoryManagementDomainModule),
    typeof(CategoryManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class CategoryManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CategoryManagementApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CategoryManagementApplicationModule>(validate: true);
        });
    }
}
