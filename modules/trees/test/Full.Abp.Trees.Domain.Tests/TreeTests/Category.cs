using System.Collections.Generic;
using Full.Abp.Trees;

namespace Full.Abp.TreeStructure.TreeTests;

public class Category : ITreeNode<Category>
{
    public int Id { get; set; }

    public int ParentId { get; set; }

    public IEnumerable<Category> Children { get; set; }
}