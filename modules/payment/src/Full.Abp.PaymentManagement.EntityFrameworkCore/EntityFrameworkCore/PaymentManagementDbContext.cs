﻿using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Full.Abp.PaymentManagement.EntityFrameworkCore;

[ConnectionStringName(PaymentManagementDbProperties.ConnectionStringName)]
public class PaymentManagementDbContext : AbpDbContext<PaymentManagementDbContext>, IPaymentManagementDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public PaymentManagementDbContext(DbContextOptions<PaymentManagementDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePaymentManagement();
    }
}
