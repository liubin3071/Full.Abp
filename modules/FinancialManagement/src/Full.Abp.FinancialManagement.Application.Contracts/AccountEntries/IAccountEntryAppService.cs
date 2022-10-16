using Volo.Abp.Application.Services;

namespace Full.Abp.FinancialManagement.AccountEntries;

public interface IAccountEntryAppService :
    IReadOnlyAppService<AccountEntryDto, AccountEntryGetListOutput, Guid, AccountEntryGetListInput>, IApplicationService
{
}