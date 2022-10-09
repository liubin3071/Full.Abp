using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Full.Abp.Trees.EntityFrameworkCore;

public static class AbpTreeDbContextModelCreatingExtensions
{
    public static void ConfigureTreeRelation<TRelation, TKey>(this EntityTypeBuilder<TRelation> b)
        where TRelation : TreeRelation<TKey>
    {
        b.ConfigureByConvention();
        
        b.HasKey(relation => new {
            relation.ProviderType,
            relation.ProviderName,
            relation.ProviderKey,
            relation.Ancestor,
            relation.Descendant,
        });
        
        // indexes
        b.HasIndex(relation => new {
            relation.ProviderType, 
            relation.ProviderName, 
            relation.ProviderKey,
            relation.Descendant
        });
        
        b.HasIndex(relation => relation.Distance);
    }
}