using Full.Abp.FinancialManagement.AccountEntries;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Full.Abp.FinancialManagement.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<AccountDto> GetAsync(AccountGetInput input);

    Task<PagedResultDto<AccountDto>> GetEntriesAsync(AccountGetListInput input);

    Task CreateOrUpdateAsync(AccountCreateOrUpdateInput input);



    Task IncreaseAsync(AccountIncreaseInput input);

    Task DecreaseAsync(AccountDecreaseInput input);

    Task<PagedResultDto<AccountEntryGetListOutput>> GetEntriesAsync(AccountEntryGetListInput input);
}