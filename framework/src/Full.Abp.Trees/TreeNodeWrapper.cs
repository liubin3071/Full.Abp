using System.Collections.Generic;

public class TreeNodeWrapper<T> : ITreeNode<TreeNodeWrapper<T>>
{
    public T Value { get; set; }
    public IEnumerable<TreeNodeWrapper<T>> Children { get; set; }
}