using System;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Guids;
using Xunit;
using Xunit.Abstractions;
using SequentialGuidGenerator = Full.Ids.SequentialGuidGenerator;

namespace Full.Abp.Ids.Tests;

public class SequentialGuidGenerator_Tests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SequentialGuidGenerator_Tests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Eq_Test()
    {
        var g1 = Guid.NewGuid();
        var g2 = new Guid(g1.ToByteArray());
        g1.ShouldBe(g2);
    }

    [Fact]
    public void Create_Test()
    {
        var sn = new SequentialGuidGenerator(1).Create();
        _testOutputHelper.WriteLine(sn.ToString());
    }


    [Fact]
    public void Order_String_Test()
    {
        var generator = new SequentialGuidGenerator(1, guidType: SequentialGuidType.SequentialAsString);
        var ids = Enumerable.Range(1, 100000).Select(c => generator.Create()).ToList();
        // var ids2 = ids.OrderBy(c => c.ToString("N"));
        ids.ShouldBeInOrder(SortDirection.Ascending);
    }
    
    
    [Fact]
    public void Order_Binary_Test()
    {
        var generator = new SequentialGuidGenerator(1, guidType: SequentialGuidType.SequentialAsString);
        var ids = Enumerable.Range(1, 100000).Select(c => generator.Create()).ToList();
        // var ids2 = ids.OrderBy(c => c.ToString("N"));
        ids.ShouldBeInOrder(SortDirection.Ascending);
    }

    // [Fact]
    // public void Order_SequentialAsBinary_Test()
    // {
    //     // todo 数据库实际验证排序
    // }
    //
    // [Fact]
    // public void Order_AtEnd_Test()
    // {
    //     // todo 数据库实际验证排序
    // }

    
    [Fact]
    public void Parse_SequentialAsString_Test()
    {
        var generator = new SequentialGuidGenerator(1, guidType: SequentialGuidType.SequentialAsString);
        var sequenceId = generator.CreateSequenceId();
        _testOutputHelper.WriteLine(sequenceId.ToString(generator.BaseTime, generator.SeqFormatLength,
            generator.RandomFormatLength, generator.WorkIdFormatLength, "-"));
        var id = generator.Encode(sequenceId);
        var sequenceId2 = generator.Parse(id);

        sequenceId.Timestamp.ShouldBe(sequenceId2.Timestamp);
        sequenceId.Random.ShouldBe(sequenceId2.Random);
        sequenceId.Seq.ShouldBe(sequenceId2.Seq);
        sequenceId.WorkId.ShouldBe(sequenceId2.WorkId);
    }
    
    [Fact]
    public void Parse_SequentialAsBinary_Test()
    {
        var generator = new SequentialGuidGenerator(1, guidType: SequentialGuidType.SequentialAsBinary);
        var sequenceId = generator.CreateSequenceId();
        _testOutputHelper.WriteLine(sequenceId.ToString(generator.BaseTime, generator.SeqFormatLength,
            generator.RandomFormatLength, generator.WorkIdFormatLength, "-"));
        var id = generator.Encode(sequenceId);
        var sequenceId2 = generator.Parse(id);

        sequenceId.Timestamp.ShouldBe(sequenceId2.Timestamp);
        sequenceId.Random.ShouldBe(sequenceId2.Random);
        sequenceId.Seq.ShouldBe(sequenceId2.Seq);
        sequenceId.WorkId.ShouldBe(sequenceId2.WorkId);
    }

    [Fact]
    public void Parse_SequentialAtEnd_Test()
    {
        var generator = new SequentialGuidGenerator(1, guidType: SequentialGuidType.SequentialAtEnd);
        var sequenceId = generator.CreateSequenceId();
        _testOutputHelper.WriteLine(sequenceId.ToString(generator.BaseTime, generator.SeqFormatLength,
            generator.RandomFormatLength, generator.WorkIdFormatLength, "-"));
        var id = generator.Encode(sequenceId);
        var sequenceId2 = generator.Parse(id);

        sequenceId.Timestamp.ShouldBe(sequenceId2.Timestamp);
        sequenceId.Random.ShouldBe(sequenceId2.Random);
        sequenceId.Seq.ShouldBe(sequenceId2.Seq);
        sequenceId.WorkId.ShouldBe(sequenceId2.WorkId);
    }
}