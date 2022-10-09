using Full.Abp.Categories;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Full.Abp.CategoryManagement;

[DependsOn(
    typeof(CategoryManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class CategoryManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(CategoryManagementApplicationContractsModule).Assembly,
            CategoryManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CategoryManagementHttpApiClientModule>();
        });

    }
}
