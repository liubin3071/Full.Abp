namespace Full.Abp.Finance.Accounts;

public interface IAccountDefinitionProvider
{
    void PreDefine(IAccountDefinitionContext context);

    void Define(IAccountDefinitionContext context);

    void PostDefine(IAccountDefinitionContext context);
}