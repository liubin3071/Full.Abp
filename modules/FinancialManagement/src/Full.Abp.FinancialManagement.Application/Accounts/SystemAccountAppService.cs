using Full.Abp.Finance.Accounts;
using Full.Abp.FinancialManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Full.Abp.FinancialManagement.Accounts;

[Authorize(FinancialManagementPermissions.SystemAccounts.Default)]
public class SystemAccountAppService : FinancialManagementAppService, ISystemAccountAppService
{
    private readonly IAccountDefinitionManager _accountDefinitionManager;
    private readonly AccountManager _accountManager;

    public SystemAccountAppService(IAccountDefinitionManager accountDefinitionManager,AccountManager accountManager)
    {
        _accountDefinitionManager = accountDefinitionManager;
        _accountManager = accountManager;
    }

    [Authorize(FinancialManagementPermissions.SystemAccounts.Default)]
    public async Task<AccountDto> GetAsync(string name)
    {
        var providerName = CurrentTenant.Id.HasValue
            ? TenantAccountProvider.ProviderName
            : GlobalAccountProvider.ProviderName;
        var account = await _accountManager.GetAsync(providerName, CurrentTenant.Id.ToString()!, name);
        return ObjectMapper.Map<Account, AccountDto>(account);
    }

    [Authorize(FinancialManagementPermissions.SystemAccounts.Default)]
    public async Task<ListResultDto<AccountDto>> GetListAsync()
    {
        var providerName = CurrentTenant.Id.HasValue
            ? TenantAccountProvider.ProviderName
            : GlobalAccountProvider.ProviderName;
        var accounts = await _accountManager.GetListAsync(providerName, CurrentTenant.Id.ToString()!);
        var dtos = ObjectMapper.Map<List<Account>, List<AccountDto>>(accounts);
        foreach (var dto in dtos)
        {
            var definition = _accountDefinitionManager.Get(dto.Name);
            dto.DisplayName = definition.DisplayName.Localize(StringLocalizerFactory).Value;
        }

        return new ListResultDto<AccountDto>(dtos);
    }
 
    [Authorize(FinancialManagementPermissions.SystemAccounts.Increase)]
    public async Task IncreaseAsync(SystemAccountIncreaseInput input)
    {
        var providerName = CurrentTenant.Id.HasValue
            ? TenantAccountProvider.ProviderName
            : GlobalAccountProvider.ProviderName;
        await _accountManager.IncreaseAsync(providerName, CurrentTenant.Id.ToString()!, input.Name, input.Amount,
            "Manual",
            Guid.NewGuid().ToString(), comments: input.Comments);
    }

    [Authorize(FinancialManagementPermissions.SystemAccounts.Decrease)]
    public async Task DecreaseAsync(SystemAccountDecreaseInput input)
    {
        var providerName = CurrentTenant.Id.HasValue
            ? TenantAccountProvider.ProviderName
            : GlobalAccountProvider.ProviderName;
        await _accountManager.DecreaseAsync(providerName, CurrentTenant.Id.ToString()!, input.Name, input.Amount,
            "Manual",
            Guid.NewGuid().ToString(), comments: input.Comments);
    }
}
