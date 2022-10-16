using Full.Abp.FinancialManagement.AccountEntries;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Full.Abp.FinancialManagement.Accounts;

[Area(FinancialManagementRemoteServiceConsts.ModuleName)]
[RemoteService(Name = FinancialManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/FinancialManagement/Accounts")]
public class AccountController : FinancialManagementController, IAccountAppService
{
    private readonly IAccountAppService _accountAppService;

    public AccountController(IAccountAppService accountAppService)
    {
        _accountAppService = accountAppService;
    }

    [HttpGet]
    public Task<AccountDto> GetAsync(AccountGetInput input)
    {
        return _accountAppService.GetAsync(input);
    }

    [HttpGet]
    [Route("Entries")]
    public Task<PagedResultDto<AccountDto>> GetEntriesAsync(AccountGetListInput input)
    {
        return _accountAppService.GetEntriesAsync(input);
    }

    [HttpPost]
    public Task CreateOrUpdateAsync(AccountCreateOrUpdateInput input)
    {
        return _accountAppService.CreateOrUpdateAsync(input);
    }

    [HttpPost]
    [Route("Increase")]
    public Task IncreaseAsync(AccountIncreaseInput input)
    {
        return _accountAppService.IncreaseAsync(input);
    }
    [HttpPost]
    [Route("Decrease")]
    public Task DecreaseAsync(AccountDecreaseInput input)
    {
        return _accountAppService.DecreaseAsync(input);
    }

    [HttpGet]
    [Route("Entries/Page")]
    public Task<PagedResultDto<AccountEntryGetListOutput>> GetEntriesAsync(AccountEntryGetListInput input)
    {
        return _accountAppService.GetEntriesAsync(input);
    }
}