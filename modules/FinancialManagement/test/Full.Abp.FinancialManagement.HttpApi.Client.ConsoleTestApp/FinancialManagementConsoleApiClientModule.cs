using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Full.Abp.FinancialManagement;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(FinancialManagementHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class FinancialManagementConsoleApiClientModule : AbpModule
{

}
