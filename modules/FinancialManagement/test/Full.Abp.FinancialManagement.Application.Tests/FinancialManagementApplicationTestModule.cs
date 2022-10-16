using Volo.Abp.Modularity;

namespace Full.Abp.FinancialManagement;

[DependsOn(
    typeof(FinancialManagementApplicationModule),
    typeof(FinancialManagementDomainTestModule)
    )]
public class FinancialManagementApplicationTestModule : AbpModule
{

}
