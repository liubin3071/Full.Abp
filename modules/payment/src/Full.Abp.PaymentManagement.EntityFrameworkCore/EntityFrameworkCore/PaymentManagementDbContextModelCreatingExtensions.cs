using Full.Abp.PaymentManagement.Payments;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Full.Abp.PaymentManagement.EntityFrameworkCore;

public static class PaymentManagementDbContextModelCreatingExtensions
{
    public static void ConfigurePaymentManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(PaymentManagementDbProperties.DbTablePrefix + "Questions", PaymentManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        builder.Entity<Payment>(b =>
        {
            //Configure table & schema name
            b.ToTable(PaymentManagementDbProperties.DbTablePrefix + "Payments", PaymentManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            // b.Property(payment => payment.Channel).IsRequired();
            b.Property(payment => payment.Title).IsRequired();

            //Relations
            b.OwnsOne(payment => payment.GatewayTransaction, navigationBuilder =>
            {
                navigationBuilder.HasIndex(tran => new {
                    tran.GatewayName,
                    tran.TransactionId
                }).IsUnique();
                
                navigationBuilder.HasIndex(tran => tran.GatewayName);
                
                navigationBuilder.HasIndex(tran => new {
                    Name = tran.GatewayName,
                    tran.ServiceProviderId
                });
                navigationBuilder.HasIndex(tran => new {
                    Name = tran.GatewayName,
                    tran.ServiceProviderId,
                    tran.MerchantId
                });
                navigationBuilder.HasIndex(tran => new {
                    Name = tran.GatewayName,
                    tran.ServiceProviderId,
                    tran.MerchantId,
                    tran.SubMerchantId
                });
            });

            b.HasOne(payment => payment.Channel).WithMany().HasForeignKey(payment => payment.ChannelId);
            b.HasMany(payment => payment.Refunds).WithOne().HasForeignKey(refund => refund.PaymentId);

            //Indexes
            b.HasIndex(payment => new {
                InChannelId = payment.ChannelId,
                payment.ChannelTransactionId
            }).IsUnique();

            // b.HasIndex(payment => new {
            //     payment.Gateway!.Name,
            //     payment.GatewayTransactionId
            // }).IsUnique();
            //
            // b.HasIndex(payment => new {
            //     payment.Gateway!.Name,
            //     payment.Gateway.ServiceProviderId,
            //     payment.Gateway.MerchantId,
            //     payment.Gateway.SubMerchantId,
            //     payment.GatewayTransactionId
            // }).IsUnique();
            //
            // b.HasIndex(payment => payment.Gateway!.Name);
            // b.HasIndex(payment => new {
            //     payment.Gateway!.Name,
            //     payment.Gateway!.ServiceProviderId
            // });
            // b.HasIndex(payment => new {
            //     payment.Gateway!.Name,
            //     payment.Gateway!.ServiceProviderId,
            //     payment.Gateway!.MerchantId
            // });
            // b.HasIndex(payment => new {
            //     payment.Gateway!.Name,
            //     payment.Gateway!.ServiceProviderId,
            //     payment.Gateway!.MerchantId,
            //     payment.Gateway!.SubMerchantId
            // });
        });


        builder.Entity<PaymentGateway>(b =>
        {
            //Configure table & schema name
            b.ToTable(PaymentManagementDbProperties.DbTablePrefix + "PaymentGateways",
                PaymentManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties


            //Relations

            //Indexes
            b.HasIndex(gateway => new {
                Name = gateway.GatewayName,
                gateway.ServiceProviderId,
                gateway.MerchantId,
                gateway.SubMerchantId
            }).IsUnique();
        });
        builder.Entity<PaymentChannel>(b =>
        {
            //Configure table & schema name
            b.ToTable(PaymentManagementDbProperties.DbTablePrefix + "PaymentChannels",
                PaymentManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(channel => channel.Name).IsRequired();

            //Relations
            b.HasMany(channel => channel.Children).WithOne(channel => channel.Parent)
                .HasForeignKey(channel => channel.ParentId);

            //Indexes
            b.HasIndex(channel => channel.Name);
        });

        builder.Entity<Refund>(b =>
        {
            //Configure table & schema name
            b.ToTable(PaymentManagementDbProperties.DbTablePrefix + "Refunds", PaymentManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties

            //Relations

            //Indexes
        });
    }
}