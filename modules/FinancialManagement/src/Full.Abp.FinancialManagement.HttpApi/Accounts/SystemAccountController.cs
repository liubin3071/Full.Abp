using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Full.Abp.FinancialManagement.Accounts;

[Area(FinancialManagementRemoteServiceConsts.ModuleName)]
[RemoteService(Name = FinancialManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/FinancialManagement/Accounts/System")]
public class SystemAccountController : FinancialManagementController, ISystemAccountAppService
{
    private readonly ISystemAccountAppService _systemAccountAppService;

    public SystemAccountController(ISystemAccountAppService systemAccountAppService)
    {
        _systemAccountAppService = systemAccountAppService;
    }

    [HttpGet]
    public Task<AccountDto> GetAsync(string name)
    {
        return _systemAccountAppService.GetAsync(name);
    }

    [HttpGet]
    [Route("All")]
    public Task<ListResultDto<AccountDto>> GetListAsync()
    {
        return _systemAccountAppService.GetListAsync();
    }

    [HttpPost]
    [Route("Increase")]
    public Task IncreaseAsync(SystemAccountIncreaseInput input)
    {
        return _systemAccountAppService.IncreaseAsync(input);
    }
    [HttpPost]
    [Route("Decrease")]
    public Task DecreaseAsync(SystemAccountDecreaseInput input)
    {
        return _systemAccountAppService.DecreaseAsync(input);
    }
}