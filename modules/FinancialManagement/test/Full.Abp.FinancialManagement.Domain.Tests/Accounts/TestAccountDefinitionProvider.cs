using Full.Abp.Finance.Accounts;

namespace Full.Abp.FinancialManagement.Accounts;

public class TestAccountDefinitionProvider : AccountDefinitionProvider
{
    public override void Define(IAccountDefinitionContext context)
    {
        context.AddAccount("Test");
    }
}