using System;
using Full.Abp.Categories;
using Full.Abp.Trees.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Full.Abp.CategoryManagement.EntityFrameworkCore;

public static class CategoryManagementDbContextModelCreatingExtensions
{
    public static void ConfigureCategoryManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(CategoryManagementDbProperties.DbTablePrefix + "Questions", CategoryManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        builder.Entity<Category>(b =>
        {
            //Configure table & schema name
            b.ToTable(CategoryManagementDbProperties.DbTablePrefix + "Categories",
                CategoryManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties

            //Relations

            //Indexes
            // b.HasIndex(category => new { category.TenantId, category.Name }).IsUnique();  // 唯一索引包含null, 不起作用

        });

        builder.Entity<CategoryTree>(b =>
        {
            //Configure table & schema name
            b.ToTable(CategoryManagementDbProperties.DbTablePrefix + "CategoryTrees",
                CategoryManagementDbProperties.DbSchema);

            b.ConfigureTree<CategoryTree, Guid>();

        });

        builder.Entity<CategoryTreeNodeRelation>(b =>
        {
            //Configure table & schema name
            b.ToTable(CategoryManagementDbProperties.DbTablePrefix + "CategoryTreeNodeRelations",
                CategoryManagementDbProperties.DbSchema);

            b.ConfigureTreeNodeRelation<CategoryTreeNodeRelation, Guid>();
        });
    }
}