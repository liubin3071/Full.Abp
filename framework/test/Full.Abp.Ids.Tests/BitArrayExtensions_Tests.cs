using System;
using System.Collections;
using System.Linq;
using System.Text.Json;
using Full.Ids;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Full.Abp.Ids.Tests;

public class BitArrayExtensions_Tests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public BitArrayExtensions_Tests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Cast_Test()
    {
        var bits = new BitArray(new bool[] { true, true, false, false });
        var l1 = bits.Cast<bool>().ToList();
        JsonSerializer.Serialize(l1).ShouldBe("[true,true,false,false]");
        _testOutputHelper.WriteLine(JsonSerializer.Serialize(l1));
        
        // var l2 = bits.Cast<int>().ToList();
        // JsonSerializer.Serialize(l1).ShouldBe("[1,1,0,0]");
        // _testOutputHelper.WriteLine(JsonSerializer.Serialize(l1));
    }

    [Fact]
    public void CopyTo_Test()
    {
        var dst = new BitArray(5);

        var src = new BitArray(new bool[] { false, true, true, false });

        src.CopyTo(1, dst, 2, 2);

        JsonSerializer.Serialize(dst).ShouldBe("[false,false,true,true,false]");
    }
    
    [Fact]
    public void CopySlice_Test()
    {
        var src = new BitArray(new bool[] { false, true, true, false ,true});

        var rst = src.CopySlice(1,3);

        JsonSerializer.Serialize(rst).ShouldBe("[true,true,false]");
    }
    
    [Fact]
    public void PadLeft_Test()
    {
        var src = new BitArray(new bool[] { true, true, false, false ,true});
        // src.Length = 10;
        // src = src.LeftShift(5);
        src.PadLeft(8,true);

        // src.Count.ShouldBe(8);
        JsonSerializer.Serialize(src).ShouldBe("[true,true,true,true,true,false,false,true]");
    }
    
    [Fact]
    public void PadRight_Test()
    {
        var src = new BitArray(new bool[] { true, true, false, false ,true});
        // src.Length = 10;
        // src = src.LeftShift(5);
        src.PadRight(8,true);

        // src.Count.ShouldBe(8);
        JsonSerializer.Serialize(src).ShouldBe("[true,true,false,false,true,true,true,true]");
    }

    [Fact]
    public void New_By_Long()
    {
        int l = 0b1111;
        var bytes = BitConverter.GetBytes(l);
        var bits = new BitArray(new int[]{l});
        
        _testOutputHelper.WriteLine(JsonSerializer.Serialize(bits));
        _testOutputHelper.WriteLine(Convert.ToString(l,2));
    }
}