using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Full.Abp.Finance.Test;

public class FinanceOptions_Tests : FinanceTestBase
{
    [Fact]
    public void Options_Test()
    {
        var options = ServiceProvider.GetRequiredService<IOptions<AbpFinanceOptions>>();
        options.Value.AccountDefinitionProviders.ShouldContain(typeof(TestAccountDefinitionProvider));
    }
}