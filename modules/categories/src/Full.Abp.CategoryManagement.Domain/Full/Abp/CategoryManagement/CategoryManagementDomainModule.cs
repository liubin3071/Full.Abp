using Full.Abp.Categories;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Full.Abp.CategoryManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CategoryManagementDomainSharedModule),
    typeof(AbpCategoriesModule)
)]
public class CategoryManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CategoryManagementDomainModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CategoryManagementDomainModule>(validate: true);
        });
    }
}