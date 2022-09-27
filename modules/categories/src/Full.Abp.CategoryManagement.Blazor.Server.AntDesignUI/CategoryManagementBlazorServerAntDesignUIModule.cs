using Full.Abp.AspnetCore.Components.Server.AntDesignTheme;
using Full.Abp.CategoryManagement.Blazor.AntDesignUI;
using Volo.Abp.Modularity;

namespace Full.Abp.CategoryManagement.Blazor.Server.AntDesignUI;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerAntDesignThemeModule),
    typeof(CategoryManagementBlazorAntDesignUiModule)
    )]
public class CategoryManagementBlazorServerAntDesignUiModule : AbpModule
{

}
