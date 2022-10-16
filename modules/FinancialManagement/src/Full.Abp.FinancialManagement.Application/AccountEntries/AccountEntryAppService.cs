using Full.Abp.FinancialManagement.Accounts;
using Full.Abp.FinancialManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;
using Full.Abp.Finance.Accounts;

namespace Full.Abp.FinancialManagement.AccountEntries;

public class AccountEntryAppService : FinancialManagementAppService, IAccountEntryAppService
{
    private readonly IRepository<AccountEntry, Guid> _entryRepository;
    private readonly AccountManager _accountManager;

    public AccountEntryAppService(IRepository<AccountEntry, Guid> entryRepository, AccountManager accountManager)
    {
        _entryRepository = entryRepository;
        _accountManager = accountManager;
    }

    public Task<AccountEntryDto> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResultDto<AccountEntryGetListOutput>> GetListAsync(AccountEntryGetListInput input)
    {
        switch (input.ProviderName)
        {
            case GlobalAccountProvider.ProviderName:
            case TenantAccountProvider.ProviderName when CurrentTenant.Id.ToString() == input.ProviderKey:
                await AuthorizationService.CheckAsync(FinancialManagementPermissions.SystemAccounts.Default);
                break;
            case UserAccountProvider.ProviderName when CurrentUser.Id.ToString() == input.ProviderKey:
                break;
            default:
                await AuthorizationService.CheckAsync(FinancialManagementPermissions.GetAccountManagementPermissions(input.ProviderName, input.Name).Default);
                break;
        }

        var account = await _accountManager.GetAsync(input.ProviderName, input.ProviderKey, input.Name);
        var query = (await _entryRepository.GetQueryableAsync())
                .Where(c => c.AccountId == account.Id)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    entry => (entry.Comments != null && entry.Comments.Contains(input.Filter!.Trim())) ||
                             entry.TransactionType.Contains(input.Filter!.Trim()) ||
                             entry.TransactionId.Contains(input.Filter.Trim()))
                .WhereIf(input.MinAmount.HasValue, entry => Math.Abs(entry.Amount) >= input.MinAmount)
                .WhereIf(input.MaxAmount.HasValue, entry => Math.Abs(entry.Amount) <= input.MaxAmount)
                .WhereIf(input.MinCreationTime.HasValue, entry => entry.CreationTime >= input.MinCreationTime)
                .WhereIf(input.MaxCreationTime.HasValue, entry => entry.CreationTime <= input.MaxCreationTime)
            ;

        var count = query.Count();
        var list = query
            .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "Id desc" : input.Sorting)
            .PageBy(input)
            .ToList();

        return new PagedResultDto<AccountEntryGetListOutput>(count,
            ObjectMapper.Map<List<AccountEntry>, List<AccountEntryGetListOutput>>(list));
    }
}