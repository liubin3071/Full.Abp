using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace Full.Abp.FinancialManagement.Samples;

[Area(FinancialManagementRemoteServiceConsts.ModuleName)]
[RemoteService(Name = FinancialManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/FinancialManagement/sample")]
public class SampleController : FinancialManagementController, ISampleAppService
{
    private readonly ISampleAppService _sampleAppService;

    public SampleController(ISampleAppService sampleAppService)
    {
        _sampleAppService = sampleAppService;
    }

    [HttpGet]
    public async Task<SampleDto> GetAsync()
    {
        return await _sampleAppService.GetAsync();
    }

    [HttpGet]
    [Route("authorized")]
    [Authorize]
    public async Task<SampleDto> GetAuthorizedAsync()
    {
        return await _sampleAppService.GetAsync();
    }
}
