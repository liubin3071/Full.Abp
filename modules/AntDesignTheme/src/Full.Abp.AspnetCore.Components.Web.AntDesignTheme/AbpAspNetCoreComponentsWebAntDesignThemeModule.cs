using Full.Abp.AntDesignUI;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.AspnetCore.Components.Web.AntDesignTheme;

[DependsOn(
    typeof(AbpAntDesignUIModule),
    typeof(AbpUiNavigationModule)
)]
public class AbpAspNetCoreComponentsWebAntDesignThemeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}
