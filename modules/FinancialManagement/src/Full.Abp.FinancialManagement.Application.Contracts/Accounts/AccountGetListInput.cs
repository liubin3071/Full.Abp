using Volo.Abp.Application.Dtos;

namespace Full.Abp.FinancialManagement.Accounts;

public class AccountGetListInput : PagedAndSortedResultRequestDto
{
    public string Name { get; set; }
    public string ProviderName { get; set; }
    public string? ProviderKey { get; set; }
    public string? Filter { get; set; }
    public decimal? MaxBalance { get; set; }
    public decimal? MinBalance { get; set; }
    public bool? IsEnabled { get; set; }
}