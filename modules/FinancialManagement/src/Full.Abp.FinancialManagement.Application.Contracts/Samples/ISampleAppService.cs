using Volo.Abp.Application.Services;

namespace Full.Abp.FinancialManagement.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
