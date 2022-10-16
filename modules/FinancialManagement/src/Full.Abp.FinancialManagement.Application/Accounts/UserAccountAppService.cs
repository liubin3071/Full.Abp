using Full.Abp.Finance.Accounts;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Full.Abp.FinancialManagement.Accounts;

[Authorize]
public class UserAccountAppService : FinancialManagementAppService,IUserAccountAppService
{
    private readonly AccountManager _accountManager;
    private readonly IAccountDefinitionManager _accountDefinitionManager;

    public UserAccountAppService(AccountManager accountManager,IAccountDefinitionManager accountDefinitionManager)
    {
        _accountManager = accountManager;
        _accountDefinitionManager = accountDefinitionManager;
    }
    
    public async Task<AccountDto> GetAsync(string name)
    {
        var account = await _accountManager.GetAsync(UserAccountProvider.ProviderName, CurrentUser.Id.ToString()!, name);
        return ObjectMapper.Map<Account, AccountDto>(account);
    }

    public async Task<ListResultDto<AccountDto>> GetListAsync()
    {
        var accounts = await _accountManager.GetListAsync(UserAccountProvider.ProviderName, CurrentUser.Id.ToString()!);
        var dtos = ObjectMapper.Map<List<Account>, List<AccountDto>>(accounts);
        foreach (var dto in dtos)
        {
            var definition = _accountDefinitionManager.Get(dto.Name);
            dto.DisplayName = definition.DisplayName.Localize(StringLocalizerFactory).Value;
        }
        return new ListResultDto<AccountDto>(dtos);
    }
}