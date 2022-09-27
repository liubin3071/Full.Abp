using Full.Abp.CategoryManagement.Full.Abp.Categories;
using Volo.Abp.Modularity;

namespace Full.Abp.CategoryManagement;

[DependsOn(
    typeof(CategoryManagementApplicationModule),
    typeof(CategoryManagementDomainTestModule)
    )]
public class CategoryManagementApplicationTestModule : AbpModule
{

}
