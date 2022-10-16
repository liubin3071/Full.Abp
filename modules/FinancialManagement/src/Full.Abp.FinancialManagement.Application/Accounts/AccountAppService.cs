using Full.Abp.Finance.Accounts;
using Full.Abp.FinancialManagement.AccountEntries;
using Full.Abp.FinancialManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;

namespace Full.Abp.FinancialManagement.Accounts;

public class AccountAppService : FinancialManagementAppService, IAccountAppService
{
    private readonly IRepository<AccountEntry, Guid> _entryRepository;
    private readonly IAccountDefinitionManager _accountDefinitionManager;
    protected IRepository<Account, Guid> AccountRepository { get; }
    protected AccountManager AccountManager { get; }

    public AccountAppService(AccountManager accountManager,
        IRepository<Account, Guid> accountRepository,
        IRepository<AccountEntry,Guid> entryRepository,
        IAccountDefinitionManager accountDefinitionManager)
    {
        _entryRepository = entryRepository;
        _accountDefinitionManager = accountDefinitionManager;
        AccountRepository = accountRepository;
        AccountManager = accountManager;
    }

    public async Task<AccountDto> GetAsync(AccountGetInput input)
    {
        await AuthorizationService.CheckAsync(FinancialManagementPermissions
            .GetAccountManagementPermissions(input.ProviderName, input.Name)
            .Default);
        var account = await AccountManager.GetAsync(input.ProviderName, input.ProviderKey, input.Name);
        return ObjectMapper.Map<Account, AccountDto>(account);
    }

    public virtual async Task<PagedResultDto<AccountDto>> GetEntriesAsync(AccountGetListInput input)
    {
        var count = await AccountManager.GetCountAsync(input.ProviderName, input.Name, input.ProviderKey,
            input.IsEnabled,
            input.MaxBalance, input.MinBalance);
        var list = await AccountManager.GetListAsync(input.ProviderName, input.Name, input.ProviderKey, input.IsEnabled,
            input.MaxBalance, input.MinBalance, input.Sorting, input.MaxResultCount, input.SkipCount);
        var dtos = ObjectMapper.Map<List<Account>, List<AccountDto>>(list);
        return new PagedResultDto<AccountDto>(count, dtos);
    }

    public async Task CreateOrUpdateAsync(AccountCreateOrUpdateInput input)
    {
        await AuthorizationService.CheckAsync(FinancialManagementPermissions
            .GetAccountManagementPermissions(input.ProviderName, input.Name)
            .Create);

        var account = await AccountManager.FindAsync(input.ProviderName, input.ProviderKey, input.Name)
                      ?? await AccountManager.CreateAsync(input.ProviderName, input.ProviderKey, input.Name);
        account.IsEnabled = input.IsEnabled;
    }


    public async Task IncreaseAsync(AccountIncreaseInput input)
    {
        await AuthorizationService.CheckAsync(FinancialManagementPermissions
            .GetAccountManagementPermissions(input.ProviderName, input.Name).Increase);
        await AccountManager.IncreaseAsync(input.ProviderName, input.ProviderKey, input.Name, input.Amount,
            "Manual",
            Guid.NewGuid().ToString(), comments: input.Comments);
    }

    public async Task DecreaseAsync(AccountDecreaseInput input)
    {
        await AuthorizationService.CheckAsync(FinancialManagementPermissions
            .GetAccountManagementPermissions(input.ProviderName, input.Name).Decrease);
        await AccountManager.DecreaseAsync(input.ProviderName, input.ProviderKey, input.Name, input.Amount,
            "Manual",
            Guid.NewGuid().ToString(), comments: input.Comments);
    }
    
    public async Task<PagedResultDto<AccountEntryGetListOutput>> GetEntriesAsync(AccountEntryGetListInput input)
    {
        await AuthorizationService.CheckAsync(FinancialManagementPermissions
            .GetAccountManagementPermissions(input.ProviderName, input.Name).Default);

        var account = await AccountManager.GetAsync(input.ProviderName, input.ProviderKey, input.Name);

        var query = (await _entryRepository.GetQueryableAsync())
                .Where(c => c.AccountId == account.Id)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    entry => (entry.Comments != null && entry.Comments.Contains(input.Filter!.Trim()))||
                             entry.TransactionType.Contains(input.Filter!.Trim())||
                             entry.TransactionId.Contains(input.Filter.Trim()))
                .WhereIf(input.MinAmount.HasValue, entry => Math.Abs(entry.Amount) >= input.MinAmount)
                .WhereIf(input.MaxAmount.HasValue, entry => Math.Abs(entry.Amount) <= input.MaxAmount)
                .WhereIf(input.MinCreationTime.HasValue, entry => entry.CreationTime >= input.MinCreationTime)
                .WhereIf(input.MaxCreationTime.HasValue, entry => entry.CreationTime >= input.MaxCreationTime)
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