using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Full.Abp.Trees.EntityFrameworkCore;

public static class AbpTreeDbContextModelCreatingExtensions
{
    public static void ConfigureTree<TTree, TKey>(this EntityTypeBuilder<TTree> b)
        where TTree : Tree<TKey>
    {
        b.HasIndex(tree => new { tree.ProviderType, tree.ProviderName, tree.ProviderKey }).IsUnique();
    }

    public static void ConfigureTreeNodeRelation<TRelation, TKey>(this EntityTypeBuilder<TRelation> b)
        where TRelation : TreeNodeRelation<TKey>
    {
        b.ConfigureByConvention();
        b.HasKey(relation => new { relation.Ancestor, relation.Descendant, relation.TreeId });
        b.HasIndex(relation => relation.Ancestor);
        b.HasIndex(relation => relation.Descendant);
    }
}