using System;
using System.Linq;
using Full.Ids;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Full.Abp.Ids.Tests;

public class SnowflakeIdGenerator_Tests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SnowflakeIdGenerator_Tests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Create_Test()
    {
        var sn = new SnowflakeIdGenerator(1).Create();
        _testOutputHelper.WriteLine(sn.ToString());
    }
    [Fact]
    public void Order_Test()
    {
        var generator = new SnowflakeIdGenerator(1);
        var ids = Enumerable.Range(1, 100000).Select(c => generator.Create()).ToList();
        // var ids2 = ids.OrderBy(c => c.ToString("N"));
        ids.ShouldBeInOrder(SortDirection.Ascending);
    }
    [Fact]
    public void Parse_Test()
    {
        var generator = new SnowflakeIdGenerator(1);
        var sequenceId = generator.CreateSequenceId();
        var id = generator.Encode(sequenceId);
        var sequenceId2 = generator.Parse(id);

        sequenceId.Timestamp.ShouldBe(sequenceId2.Timestamp);
        sequenceId.Random.ShouldBe(sequenceId2.Random);
        sequenceId.Seq.ShouldBe(sequenceId2.Seq);
        sequenceId.WorkId.ShouldBe(sequenceId2.WorkId);
    }
}