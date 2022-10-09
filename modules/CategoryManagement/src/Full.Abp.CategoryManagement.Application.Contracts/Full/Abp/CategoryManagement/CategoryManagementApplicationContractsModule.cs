using Full.Abp.Categories;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Full.Abp.CategoryManagement;

[DependsOn(
    typeof(CategoryManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpCategoriesModule)
    )]
public class CategoryManagementApplicationContractsModule : AbpModule
{

}
