using Full.Abp.CategoryManagement;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Full.Abp.Categories;

[DependsOn(
    typeof(CategoryManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class CategoryManagementApplicationContractsModule : AbpModule
{

}
