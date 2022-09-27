using System;
using System.Collections.Generic;
using System.Linq;

public static class TreeExtensions
{
    public static IEnumerable<TTreeNode> ToTree<TTreeNode>(this IEnumerable<TTreeNode> sources,
        Func<TTreeNode, TTreeNode?> parentSelector, TTreeNode? rootId = default,
        IEqualityComparer<TTreeNode?>? comparer = null) where TTreeNode : ITreeNode<TTreeNode>
    {
        var lookup = sources.ToLookup(parentSelector, comparer ?? EqualityComparer<TTreeNode?>.Default);
        var roots = lookup[rootId].ToList();
        foreach (var root in roots)
        {
            LoadChildren(root, lookup);
        }

        return roots;
    }

    public static IEnumerable<TTreeNode> ToTree<TTreeNode, TKey>(this IEnumerable<TTreeNode> sources,
        Func<TTreeNode, TKey?> parentKeySelector, Func<TTreeNode, TKey> keySelector,
        TKey? rootKey = default, IEqualityComparer<TKey?>? keyComparer = null)
        where TTreeNode : ITreeNode<TTreeNode>
    {
        var lookup = sources.ToLookup(parentKeySelector, keyComparer ?? EqualityComparer<TKey?>.Default);
        var roots = lookup[rootKey].ToList();
        foreach (var root in roots)
        {
            LoadChildren(root, keySelector, lookup);
        }

        return roots;
    }

    public static List<TTreeNode> ToTree<TSource, TKey, TTreeNode>(this IEnumerable<TSource> sources,
        Func<TSource, TKey?> parentKeySelector, Func<TTreeNode, TKey?> keySelector, Func<TSource, TTreeNode> convertor,
        TKey? rootKey = default, IEqualityComparer<TKey?>? keyComparer = default)
        where TTreeNode : ITreeNode<TTreeNode>
    {
        var comparer = keyComparer ?? EqualityComparer<TKey?>.Default;
        var lookup = sources.ToLookup(parentKeySelector, convertor, comparer);
        var roots = lookup[rootKey].ToList();
        foreach (var root in roots)
        {
            LoadChildren<TKey, TTreeNode>(root, keySelector, lookup);
        }

        return roots;
    }

    private static void LoadChildren<TTreeNode>(TTreeNode node, ILookup<TTreeNode?, TTreeNode> lookup)
        where TTreeNode : ITreeNode<TTreeNode>
    {
        node.Children = lookup[node];
        foreach (var child in node.Children)
        {
            LoadChildren(child, lookup);
        }
    }

    private static void LoadChildren<TKey, TTreeNode>(TTreeNode node, Func<TTreeNode, TKey?> keySelector,
        ILookup<TKey?, TTreeNode> lookup)
        where TTreeNode : ITreeNode<TTreeNode>
    {
        node.Children = lookup[keySelector(node)];
        foreach (var child in node.Children)
        {
            LoadChildren(child, keySelector, lookup);
        }
    }
    public static IEnumerable<TResult> TreeSelect<TSource, TResult>(this IEnumerable<TSource> tree,
        Func<TSource, TResult> selector, Func<TSource, IEnumerable<TSource>> childrenSelector, Action<TResult,IEnumerable<TResult>> setChildren)
    {
        return tree.Select(c =>
        {
            var d = selector(c);
            setChildren(d, childrenSelector(c).TreeSelect<TSource, TResult>(selector, childrenSelector,setChildren));
            return d;
        });
    }
    
    public static IEnumerable<TResult> TreeSelect<TSource, TResult>(this IEnumerable<TSource> tree,
        Func<TSource, TResult> selector, Func<TSource, IEnumerable<TSource>> childrenSelector)
        where TResult : ITreeNode<TResult>
    {
        return tree.Select(c =>
        {
            var d = selector(c);
            d.Children = childrenSelector(c).TreeSelect<TSource, TResult>(selector, childrenSelector);
            return d;
        });
    }

    public static IEnumerable<TResult> TreeSelect<TSource, TResult>(this IEnumerable<TSource> tree,
        Func<TSource, TResult> selector)
        where TSource : ITreeNode<TSource>
        where TResult : ITreeNode<TResult>
    {
        return tree.Select(c =>
        {
            var d = selector(c);
            d.Children = c.Children.TreeSelect<TSource, TResult>(selector);
            return d;
        });
    }
}