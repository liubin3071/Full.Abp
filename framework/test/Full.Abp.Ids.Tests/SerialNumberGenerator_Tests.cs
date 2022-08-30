using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Full.Ids;
using Nito.AsyncEx;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Full.Abp.Ids.Tests;

public class SerialNumberGenerator_Tests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SerialNumberGenerator_Tests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Create_Test()
    {
        var sn = new SerialNumberGenerator(1).Create();
        _testOutputHelper.WriteLine(sn);
    }
    
    [Fact]
    public void Order_Test()
    {
        var generator = new SerialNumberGenerator(1);
        var ids = Enumerable.Range(1, 100000).Select(c => generator.Create()).ToList();
        // var ids2 = ids.OrderBy(c => c.ToString("N"));
        ids.ShouldBeInOrder(SortDirection.Ascending);
    }
    
    [Fact]
    public void Parse_Test()
    {
        var generator = new SerialNumberGenerator(1);
        var id = generator.Create();
        var s = generator.Parse(id);
        var id2 = s.ToString(generator.BaseTime, generator.SeqFormatLength, generator.RandomFormatLength,generator.WorkIdFormatLength, generator.Separator);
        id.ShouldBe(id2);
    }


    [Fact]
    public async Task Next_Concurrent_Test()
    {
        var g = new SerialNumberGenerator(1);
        var tasks = Enumerable.Range(1, 1000)
            .Select(c => new Task<List<string>>(() =>
            {
                var list = new List<string>();
                for (var i = 0; i < 100; i++)
                {
                    list.Add(g.Create());
                }

                return list;
            }))
            .ToList();

        tasks.ForEach(c => c.Start());
        var results = (await tasks.WhenAll()).SelectMany(c => c).ToList();

        _testOutputHelper.WriteLine(results.Count.ToString());
        results.OrderBy(c => c).Take(5000).ToList().ForEach(l => _testOutputHelper.WriteLine(l));
        results.Distinct().Count().ShouldBe(results.Count);
    }
}