using System.Linq.Dynamic.Core;
using Full.Abp.Finance.Accounts;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace Full.Abp.FinancialManagement.Accounts;

public class AccountManager : DomainService, ITransientDependency
{
    private readonly IRepository<Account, Guid> _accountRepository;
    private readonly IRepository<AccountEntry, Guid> _entryRepository;
    private readonly IAccountDefinitionManager _accountDefinitionManager;

    public AccountManager(IRepository<Account, Guid> accountRepository, IRepository<AccountEntry, Guid> entryRepository,
        IAccountDefinitionManager accountDefinitionManager)
    {
        _accountRepository = accountRepository;
        _entryRepository = entryRepository;
        _accountDefinitionManager = accountDefinitionManager;
    }

    private async Task<IQueryable<Account>> GetQueryable(string providerName, string name, string? providerKey = null,
        bool? enable = null, decimal? maxBalance = null, decimal? minBalance = null)
    {
        return (await _accountRepository.GetQueryableAsync())
            .Where(c => c.ProviderName == providerName && c.Name == name)
            .WhereIf(!string.IsNullOrEmpty(providerKey), account => account.ProviderKey == providerKey)
            .WhereIf(enable.HasValue, account => account.IsEnabled == enable)
            .WhereIf(maxBalance.HasValue, account => account.Balance <= maxBalance)
            .WhereIf(minBalance.HasValue, account => account.Balance >= minBalance)
            ;
    }

    public async Task<int> GetCountAsync(string providerName, string name, string? providerKey = null,
        bool? isEnabled = null, decimal? maxBalance = null, decimal? minBalance = null)
    {
        var queryable = await GetQueryable(providerName, name, providerKey, isEnabled, maxBalance, minBalance);
        return queryable.Count();
    }

    public async Task<List<Account>> GetListAsync(string providerName, string name, string? providerKey = null,
        bool? isEnabled = null, decimal? maxBalance = null, decimal? minBalance = null, string? sorting = null,
        int maxResultCount = 10, int skipCount = 0)
    {
        var definition = _accountDefinitionManager.Get(name);

        var queryable = await GetQueryable(providerName, name, providerKey, isEnabled, maxBalance, minBalance);

        return queryable
            .OrderBy(sorting.IsNullOrWhiteSpace() ? "Id desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToList();
    }

    public async Task<List<Account>> GetListAsync(string providerName, string providerKey)
    {
        var names = _accountDefinitionManager.GetAll()
            .Where(c => c.IsAllowedProvider(providerName))
            .Select(c => c.Name);

        return (await _accountRepository.GetQueryableAsync())
            .Where(c => c.ProviderName == providerName && c.ProviderKey == providerKey && names.Contains(c.Name))
            .ToList();
    }

    public async Task<Account> CreateAsync(string providerName, string providerKey, string name)
    {
        var definition = _accountDefinitionManager.Get(name);
        CheckProvider(definition, providerName);

        var account = await FindAsync(providerName, providerKey, name);
        if (account != null)
        {
            throw new BusinessException(FinancialManagementErrorCodes.AccountAlreadyExists);
        }

        account = new Account(providerName, providerKey, name);
        await _accountRepository.InsertAsync(account);

        return account;
    }


    [UnitOfWork]
    public virtual async Task<Guid> DecreaseAsync(string providerName, string providerKey, string name, decimal amount,
        string transactionType, string transactionId, string? comments = null, IDictionary<string, object>? data = null,
        CancellationToken cancellationToken = default)
    {
        var definition = _accountDefinitionManager.Get(name);
        CheckProvider(definition, providerName);

        var amountValue = Math.Round(amount, definition.Precision);
        if (amountValue <= 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        var account = await GetAsync(providerName, providerKey, name);
        if (!account.IsEnabled)
        {
            throw new InvalidOperationException("AccountDisabled");
        }

        var postBalance = account.Balance - amountValue;
        if (postBalance < 0)
        {
            throw new InvalidOperationException("Insufficient Balance.");
        }

        account.Balance = postBalance;
        account.LatestEntryIndex++;
        var entry = new AccountEntry(account.Id, account.LatestEntryIndex, -amountValue, postBalance, transactionType,
            transactionId, comments);

        if (!data.IsNullOrEmpty())
        {
            foreach (var item in data!)
            {
                entry.ExtraProperties.Add(item.Key, item.Value);
            }
        }

        await _entryRepository.InsertAsync(entry, false, cancellationToken);
        return entry.Id;
    }

    [UnitOfWork]
    public virtual async Task<Guid> IncreaseAsync(string providerName, string providerKey, string name,
        decimal amount, string transactionType, string transactionId, string? comments = null,
        IDictionary<string, object>? data = null,
        CancellationToken cancellationToken = default)
    {
        var definition = _accountDefinitionManager.Get(name);
        CheckProvider(definition, providerName);


        var amountLong = (long)Math.Round(amount);
        if (amountLong <= 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        var account = await GetAsync(providerName, providerKey, name);
        if (!account.IsEnabled)
        {
            throw new InvalidOperationException("AccountDisabled");
        }

        var postBalance = account.Balance + amountLong;
        account.Balance = postBalance;
        account.LatestEntryIndex++;
        var entry = new AccountEntry(account.Id, account.LatestEntryIndex, amountLong, postBalance, transactionType,
            transactionId, comments);
        if (!data.IsNullOrEmpty())
        {
            foreach (var item in data!)
            {
                entry.ExtraProperties.Add(item.Key, item.Value);
            }
        }

        await _entryRepository.InsertAsync(entry, false, cancellationToken);
        return entry.Id;
    }

    public virtual async Task<Guid?> FindIdAsync(string providerName, string providerKey, string name)
    {
        var definition = _accountDefinitionManager.Get(name);
        CheckProvider(definition, providerName);

        return (await _accountRepository.GetQueryableAsync())
            .Where(c => c.ProviderName == providerName && c.ProviderKey == providerKey && c.Name == name)
            .Select(account => (Guid?)account.Id)
            .FirstOrDefault();
    }

    public virtual async Task<Account> GetAsync(string providerName, string providerKey, string name)
    {
        var definition = _accountDefinitionManager.Get(name);
        CheckProvider(definition, providerName);

        var account = await FindAsync(providerName, providerKey, name);
        if (account == null)
        {
            throw new EntityNotFoundException(typeof(Account));
        }

        return account;
    }

    public virtual async Task<Account?> FindAsync(string providerName,
        string providerKey, string name)
    {
        var definition = _accountDefinitionManager.Get(name);
        CheckProvider(definition, providerName);

        return (await _accountRepository.GetQueryableAsync())
            .FirstOrDefault(c => c.ProviderName == providerName && c.ProviderKey == providerKey && c.Name == name);
    }

    public virtual async Task<decimal> GetBalanceAsync(string providerName, string providerKey, string name)
    {
        var definition = _accountDefinitionManager.Get(name);
        CheckProvider(definition, providerName);


        var account = await GetAsync(providerName, providerKey, name);
        return account.Balance;
    }

    public virtual async Task<IDictionary<string, decimal>> GetBalancesAsync(string providerName, string providerKey,
        params string[] names)
    {
        foreach (var name in names)
        {
            var definition = _accountDefinitionManager.Get(name);
            CheckProvider(definition, providerName);
        }

        var queryable = (await _accountRepository.GetQueryableAsync())
                .Where(c => c.ProviderName == providerName && c.ProviderKey == providerKey)
            ;

        return (from accountName in names
                join account in queryable on accountName equals account.Name
                select new { AccountName = accountName, Balance = account.Balance }
            )
            .ToDictionary(c => c.AccountName, c => c.Balance);
    }

    /// <summary>
    /// 验证账户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task Verify(Guid id)
    {
        var account = await _accountRepository.GetAsync(id, true);
        var entries = (await _entryRepository.GetQueryableAsync())
            .Where(c => c.AccountId == id)
            .OrderBy(c => c.Index)
            .ToList();

        var last = entries.Last();

        if (account.Balance != last.PostBalance)
        {
            throw new InvalidOperationException("账户与最新条目余额不一致.");
        }

        VerifyEntries(entries);
    }


    /// <summary>
    /// 验证条目索引及余额连续性
    /// </summary>
    /// <param name="entries">必须预先按照索引升序排序</param>
    /// <exception cref="InvalidOperationException"></exception>
    private static void VerifyEntries(IReadOnlyList<AccountEntry> entries)
    {
        if (entries.Count <= 1)
        {
            return;
        }

        for (var i = 0; i < entries.Count - 1; i++)
        {
            var e1 = entries[i];
            var e2 = entries[i + 1];
            if (e2.Index != e1.Index + 1)
            {
                throw new InvalidOperationException("条目索引不连续.");
            }

            if (e1.PostBalance + e2.Amount != e2.PostBalance)
            {
                throw new InvalidOperationException("条目余额不连续.");
            }
        }
    }

    private void CheckProvider(AccountDefinition definition, string providerName)
    {
        if (!definition.IsAllowedProvider(providerName))
        {
            throw new InvalidOperationException(
                $"Provider '{providerName}' is not allowed for the AccountDefinition named '{definition.Name}' ");
        }
    }
}