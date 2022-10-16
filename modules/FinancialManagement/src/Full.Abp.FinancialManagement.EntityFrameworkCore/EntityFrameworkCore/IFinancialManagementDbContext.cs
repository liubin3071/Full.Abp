using Full.Abp.FinancialManagement.Accounts;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Full.Abp.FinancialManagement.EntityFrameworkCore;

[ConnectionStringName(FinancialManagementDbProperties.ConnectionStringName)]
public interface IFinancialManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */

    DbSet<Account> Accounts { get; }
    DbSet<AccountEntry> AccountEntries { get; }
}