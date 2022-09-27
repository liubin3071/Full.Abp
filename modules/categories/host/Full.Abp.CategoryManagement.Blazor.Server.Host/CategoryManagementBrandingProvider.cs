using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Full.Abp.CategoryDemo.Blazor.Server;

[Dependency(ReplaceServices = true)]
public class CategoryManagementBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "CategoryManagement";
}
