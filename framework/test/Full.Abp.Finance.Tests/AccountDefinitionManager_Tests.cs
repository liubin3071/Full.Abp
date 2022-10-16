using Full.Abp.Finance.Accounts;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Full.Abp.Finance.Test;

public class AccountDefinitionManager_Tests : FinanceTestBase
{
    [Fact]
    public void Get_Test()
    {
        var manager = ServiceProvider.GetRequiredService<IAccountDefinitionManager>();

        var testAccount = manager.Get("Test");
        testAccount.Name.ShouldBe("Test");
    }

    [Fact]
    public void GetOrNull_Test()
    {
        var manager = ServiceProvider.GetRequiredService<IAccountDefinitionManager>();

        var testAccount = manager.GetOrNull("Test");
        testAccount.ShouldNotBeNull();
        testAccount.Name.ShouldBe("Test");
    }

    [Fact]
    public void GetAccounts_Test()
    {
        var manager = ServiceProvider.GetRequiredService<IAccountDefinitionManager>();
        var accounts = manager.GetAll();
        accounts.ShouldContain(a => a.Name == "Test");
    }
}