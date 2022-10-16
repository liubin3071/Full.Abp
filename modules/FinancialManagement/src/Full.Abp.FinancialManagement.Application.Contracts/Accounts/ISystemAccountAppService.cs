using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Full.Abp.FinancialManagement.Accounts;

public interface ISystemAccountAppService : IApplicationService
{
    Task<AccountDto> GetAsync(string name);

    Task<ListResultDto<AccountDto>> GetListAsync();

    Task IncreaseAsync(SystemAccountIncreaseInput input);

    Task DecreaseAsync(SystemAccountDecreaseInput input);
}