using System.Collections.Generic;

public interface ITreeNode<T>
{
    public IEnumerable<T> Children { get; set; }
}