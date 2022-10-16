using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Full.Abp.FinancialManagement.Accounts;

[Area(FinancialManagementRemoteServiceConsts.ModuleName)]
[RemoteService(Name = FinancialManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/Account/FinancialManagement/Accounts")]
public class UserAccountController : FinancialManagementController, IUserAccountAppService
{
    private readonly IUserAccountAppService _userAccountAppService;

    public UserAccountController(IUserAccountAppService userAccountAppService)
    {
        _userAccountAppService = userAccountAppService;
    }

    [HttpGet]
    public Task<AccountDto> GetAsync(string name)
    {
        return _userAccountAppService.GetAsync(name);
    }

    [HttpGet]
    [Route("All")]
    public Task<ListResultDto<AccountDto>> GetListAsync()
    {
        return _userAccountAppService.GetListAsync();
    }
}