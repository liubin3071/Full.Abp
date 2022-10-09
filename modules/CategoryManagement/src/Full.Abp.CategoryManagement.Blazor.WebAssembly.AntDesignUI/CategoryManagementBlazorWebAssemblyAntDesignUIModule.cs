using Full.Abp.CategoryManagement.Blazor.AntDesignUI;
using Volo.Abp.Modularity;

namespace Full.Abp.CategoryManagement.Blazor.WebAssembly.AntDesignUI;

[DependsOn(
    typeof(CategoryManagementBlazorAntDesignUiModule),
    typeof(CategoryManagementHttpApiClientModule)
    )]
public class CategoryManagementBlazorWebAssemblyAntDesignUiModule : AbpModule
{

}
