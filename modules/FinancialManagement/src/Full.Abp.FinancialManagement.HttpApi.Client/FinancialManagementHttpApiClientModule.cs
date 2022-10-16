using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Full.Abp.FinancialManagement;

[DependsOn(
    typeof(FinancialManagementApplicationContractsModule),
    typeof(AbpHttpClientModule)
)]
public class FinancialManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(FinancialManagementApplicationContractsModule).Assembly,
            FinancialManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FinancialManagementHttpApiClientModule>();
        });
    }
}