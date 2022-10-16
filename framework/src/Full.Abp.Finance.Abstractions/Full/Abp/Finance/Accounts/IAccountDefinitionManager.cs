namespace Full.Abp.Finance.Accounts;

public interface IAccountDefinitionManager
{
    AccountDefinition Get(string name);

    AccountDefinition GetOrNull(string name);

    IReadOnlyList<AccountDefinition> GetAll();

}