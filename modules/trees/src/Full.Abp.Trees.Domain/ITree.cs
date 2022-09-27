using System.Collections.Generic;

namespace Full.Abp.Trees;

public interface ITree<T>
{
    T Data { get; }
    ITree<T> Parent { get; }
    ICollection<ITree<T>> Children { get; }
    bool IsRoot { get; }
    bool IsLeaf { get; }
    int Level { get; }
}