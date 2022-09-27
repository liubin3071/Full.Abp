using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Full.Abp.CategoryManagement;

[Dependency(ReplaceServices = true)]
public class CategoryManagementBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "CategoryManagement";
}
