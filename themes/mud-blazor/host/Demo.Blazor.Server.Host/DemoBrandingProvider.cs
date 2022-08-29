using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Demo.Blazor.Server.Host;

[Dependency(ReplaceServices = true)]
public class DemoBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Demo";
}