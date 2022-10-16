using Volo.Abp.DependencyInjection;

namespace Full.Abp.Finance.Accounts;

public abstract class AccountDefinitionProvider : IAccountDefinitionProvider, ITransientDependency
{
    public virtual void PreDefine(IAccountDefinitionContext context)
    {
    }

    public abstract void Define(IAccountDefinitionContext context);

    public virtual void PostDefine(IAccountDefinitionContext context)
    {
    }
}