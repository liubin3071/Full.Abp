using Volo.Abp.Studio;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Full.Abp.CategoryManagement;

[DependsOn(
    typeof(AbpStudioModuleInstallerModule),
    typeof(AbpVirtualFileSystemModule)
    )]
public class CategoryManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CategoryManagementInstallerModule>();
        });
    }
}
