using AutoMapper;
using Full.Abp.FinancialManagement.AccountEntries;
using Full.Abp.FinancialManagement.Accounts;
using Volo.Abp.AutoMapper;

namespace Full.Abp.FinancialManagement;

public class FinancialManagementApplicationAutoMapperProfile : Profile
{
    public FinancialManagementApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Account, AccountDto>(MemberList.Destination)
            .Ignore(c => c.DisplayName);
        CreateMap<AccountEntry, AccountEntryGetListOutput>(MemberList.Destination);
        CreateMap<AccountEntry, AccountEntryDto>(MemberList.Destination);
    }
}
