using System;
using System.Threading.Tasks;
using Full.Abp.Finance.Accounts;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace Full.Abp.FinancialManagement.Accounts;

public class AccountStoreTests : FinancialManagementDomainTestBase
{
    public IAccountStore AccountStore { get; set; }

    public AccountStoreTests()
    {
        AccountStore = ServiceProvider.GetRequiredService<IAccountStore>();
    }

    private async Task CreateAccount(decimal initBalance = 0)
    {
        var repository = ServiceProvider.GetRequiredService<IRepository<Account, Guid>>();
        await repository.InsertAsync(new Account("Test", "Test", "Test", true), true);

        if (initBalance > 0)
        {
            await AccountStore.IncreaseAsync("Test", "Test", "Test", initBalance, "init", Guid.NewGuid().ToString());
        }
    }

    [Fact]
    public async Task Get_Account_Test()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            await CreateAccount(100);

            var balance = await AccountStore.GetBalanceAsync("Test", "Test", "Test");

            // assert
            balance.ShouldBe(100m);
        });
    }

    [Fact]
    public async Task Income_Test()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            await CreateAccount();
            await AccountStore.IncreaseAsync("Test", "Test", "Test", 100, "test", Guid.NewGuid().ToString(), "Test");

            // assert
            var balance = await AccountStore.GetBalanceAsync("Test", "Test", "Test");
            balance.ShouldBe(100);
        });
    }

    [Fact]
    public async Task Expense_Test()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            await CreateAccount(100);
            await AccountStore.DecreaseAsync("Test", "Test", "Test", 10, "test", Guid.NewGuid().ToString(), "Test");

            // assert
            var balance = await AccountStore.GetBalanceAsync("Test", "Test", "Test");
            balance.ShouldBe(90);
        });
    }
}