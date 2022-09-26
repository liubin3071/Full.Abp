using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace BookStore.BlazorServer;

[Dependency(ReplaceServices = true)]
public class BookStoreBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "BookStore";
}
