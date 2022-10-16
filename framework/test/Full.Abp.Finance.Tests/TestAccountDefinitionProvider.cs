using Full.Abp.Finance.Accounts;
using Volo.Abp.DependencyInjection;

namespace Full.Abp.Finance.Test;

public class TestAccountDefinitionProvider : AccountDefinitionProvider, ITransientDependency
{
    public override void Define(IAccountDefinitionContext context)
    {
        context.AddAccount("Test");
    }
}