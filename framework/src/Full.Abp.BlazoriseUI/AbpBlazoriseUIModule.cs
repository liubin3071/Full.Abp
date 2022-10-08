using Blazorise;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Full.Abp.BlazoriseUI;

[DependsOn(
    typeof(Volo.Abp.BlazoriseUI.AbpBlazoriseUIModule)
)]
public class AbpBlazoriseUIModule : AbpModule
{
}