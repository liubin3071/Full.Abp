using Volo.Abp.Application.Dtos;

namespace Full.Abp.FinancialManagement.AccountEntries;

public class AccountEntryGetListInput : PagedAndSortedResultRequestDto
{
    public string ProviderName { get; set; }

    public string ProviderKey { get; set; }

    public string Name { get; set; }

    public string? Filter { get; set; }

    public decimal? MinAmount { get; set; }

    public decimal? MaxAmount { get; set; }

    public DateTime? MinCreationTime { get; set; }

    public DateTime? MaxCreationTime { get; set; }
}