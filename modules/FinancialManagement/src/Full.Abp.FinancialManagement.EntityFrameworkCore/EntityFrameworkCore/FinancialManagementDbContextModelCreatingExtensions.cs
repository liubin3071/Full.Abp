using Full.Abp.FinancialManagement.Accounts;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Full.Abp.FinancialManagement.EntityFrameworkCore;

public static class FinancialManagementDbContextModelCreatingExtensions
{
    public static void ConfigureFinancialManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(FinancialManagementDbProperties.DbTablePrefix + "Questions", FinancialManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        builder.Entity<Account>(b =>
        {
            //Configure table & schema name
            b.ToTable(FinancialManagementDbProperties.DbTablePrefix + "Accounts", FinancialManagementDbProperties.DbSchema);
            b.ConfigureByConvention();

            //Properties
            b.Property(c => c.Balance).HasColumnType("DECIMAL(19,4)");

            //Relations
            b.HasMany<AccountEntry>().WithOne()
                .HasForeignKey(book => book.AccountId)
                .IsRequired()
                ;

            // Indexes
            b.HasIndex(account => new { account.TenantId, account.ProviderName, account.ProviderKey, account.Name })
                .IsUnique();
        });

        builder.Entity<AccountEntry>(b =>
        {
            //Configure table & schema name
            b.ToTable(FinancialManagementDbProperties.DbTablePrefix + "AccountEntries", FinancialManagementDbProperties.DbSchema);
            b.ConfigureByConvention();

            //Properties
            b.Property(c => c.Amount).HasColumnType("DECIMAL(19,4)");
            b.Property(c => c.PostBalance).HasColumnType("DECIMAL(19,4)");

            // Indexes
            b.HasIndex(entry => new { entry.AccountId, entry.Index }).IsUnique();

            // Properties
            b.Property(entry => entry.Amount).IsRequired();
        });
    }
}