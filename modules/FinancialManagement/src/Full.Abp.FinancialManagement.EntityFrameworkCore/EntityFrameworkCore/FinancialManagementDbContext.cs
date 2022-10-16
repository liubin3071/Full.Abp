using Full.Abp.FinancialManagement.Accounts;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Full.Abp.FinancialManagement.EntityFrameworkCore;

[ConnectionStringName(FinancialManagementDbProperties.ConnectionStringName)]
public class FinancialManagementDbContext : AbpDbContext<FinancialManagementDbContext>, IFinancialManagementDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */


    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<AccountEntry> AccountEntries { get; set; } = null!;

    public FinancialManagementDbContext(DbContextOptions<FinancialManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureFinancialManagement();
    }
}