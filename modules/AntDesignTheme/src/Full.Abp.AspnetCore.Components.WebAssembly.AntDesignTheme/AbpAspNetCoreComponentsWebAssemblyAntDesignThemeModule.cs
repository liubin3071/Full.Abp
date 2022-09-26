﻿using Full.Abp.AspnetCore.Components.Web.AntDesignTheme;
using Full.Abp.AspnetCore.Components.Web.AntDesignTheme.Routing;
using Full.Abp.AspnetCore.Components.Web.AntDesignTheme.Toolbars;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.Http.Client.IdentityModel.WebAssembly;
using Volo.Abp.Modularity;

namespace Full.Abp.AspnetCore.Components.WebAssembly.AntDesignTheme;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebAntDesignThemeModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyModule),
    typeof(AbpHttpClientIdentityModelWebAssemblyModule)
)]
public class AbpAspNetCoreComponentsWebAssemblyAntDesignThemeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpAspNetCoreComponentsWebAssemblyAntDesignThemeModule).Assembly);
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new AntDesignThemeToolbarContributor());
        });
    }
}
