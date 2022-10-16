using Full.Abp.Finance.Accounts;
using Volo.Abp.Collections;

namespace Full.Abp.Finance;

public class AbpFinanceOptions
{
    public ITypeList<IAccountDefinitionProvider> AccountDefinitionProviders { get; }

    public AbpFinanceOptions()
    {
        AccountDefinitionProviders = new TypeList<IAccountDefinitionProvider>();
    }
}