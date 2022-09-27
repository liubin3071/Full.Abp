#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Full.Abp.Trees;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Full.Abp.TreeStructure.TreeTests;

public class TreeExtensionsTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TreeExtensionsTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void ToTree_Wrapper_Test()
    {
        var list = TestData.GetList().OrderBy(c => c.Id.ToString()).ToList();

        var tree = list.ToTree(category => category.ParentId, category => category.Value.Id,
            category => new TreeNodeWrapper<Category>() { Value = category }, 0, null);
        var list2 = new List<TreeNodeWrapper<Category>>();
        TreeToList(tree, list2);

        list.Count.ShouldBe(list2.Count);
        for (var i = 0; i < list.Count; i++)
        {
            list[i].ShouldBe(list2[i].Value);
            _testOutputHelper.WriteLine($"{list[i].Id}\t\t{list2[i].Value.Id}");
        }
        
        // list.ToTree(category => category.ParentId, category => category.Id, category => category);
        // list.ToTree(category => category.ParentId, category => category.Id);
        // list.ToTree(category => list.FirstOrDefault(c => c.Id == category.ParentId));
    }
    
    [Fact]
    public void ToTree_Test1()
    {
        var list = TestData.GetList().OrderBy(c => c.Id.ToString()).ToList();

        var tree = list.ToTree(category => category.ParentId, category => category.Id, category => category);
        var list2 = new List<Category>();
        TreeToList(tree, list2);

        list.Count.ShouldBe(list2.Count);
        for (var i = 0; i < list.Count; i++)
        {
            list[i].ShouldBe(list2[i]);
            _testOutputHelper.WriteLine($"{list[i].Id}\t\t{list2[i].Id}");
        }
    }
    
    [Fact]
    public void ToTree_Test2()
    {
        var list = TestData.GetList().OrderBy(c => c.Id.ToString()).ToList();

        var tree = list.ToTree(category => category.ParentId, category => category.Id);
        var list2 = new List<Category>();
        TreeToList(tree, list2);

        list.Count.ShouldBe(list2.Count);
        for (var i = 0; i < list.Count; i++)
        {
            list[i].ShouldBe(list2[i]);
            _testOutputHelper.WriteLine($"{list[i].Id}\t\t{list2[i].Id}");
        }
    }
    
    [Fact]
    public void ToTree_Test3()
    {
        var list = TestData.GetList().OrderBy(c => c.Id.ToString()).ToList();

        var tree = list.ToTree(category => list.FirstOrDefault(c => c.Id == category.ParentId));
        var list2 = new List<Category>();
        TreeToList(tree, list2);

        list.Count.ShouldBe(list2.Count);
        for (var i = 0; i < list.Count; i++)
        {
            list[i].ShouldBe(list2[i]);
            _testOutputHelper.WriteLine($"{list[i].Id}\t\t{list2[i].Id}");
        }
    }

    private void TreeToList<T>(IEnumerable<T> tree, ICollection<T> result) where T : ITreeNode<T>
    {
        foreach (var node in tree)
        {
            result.Add(node);
            TreeToList<T>(node.Children, result);
        }
    }
}
