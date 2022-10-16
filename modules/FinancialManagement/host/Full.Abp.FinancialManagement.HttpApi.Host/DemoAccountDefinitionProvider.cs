using Full.Abp.Finance.Accounts;
using Full.Abp.FinancialManagement.Localization;
using Volo.Abp.Localization;

namespace Full.Abp.FinancialManagement;

public class DemoAccountDefinitionProvider : AccountDefinitionProvider
{
    public override void Define(IAccountDefinitionContext context)
    {
        context.AddAccount("CashAccount",L("Accounts:CashAccount"));
        context.AddAccount("Point",L("Accounts:Point"));
    }
    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FinancialManagementResource>(name);
    }
}