using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Full.Abp.PaymentManagement;

[DependsOn(
    typeof(PaymentManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class PaymentManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(PaymentManagementApplicationContractsModule).Assembly,
            PaymentManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PaymentManagementHttpApiClientModule>();
        });

    }
}
