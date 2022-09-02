using Demo.Blazor.Server.Host.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Demo.Blazor.Server.Host;

public abstract class DemoComponentBase : AbpComponentBase
{
    protected DemoComponentBase()
    {
        LocalizationResource = typeof(DemoResource);
    }
}