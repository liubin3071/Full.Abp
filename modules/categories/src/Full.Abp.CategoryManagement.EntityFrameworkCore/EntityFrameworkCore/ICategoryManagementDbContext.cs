using Full.Abp.Categories;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Full.Abp.CategoryManagement.EntityFrameworkCore;

[ConnectionStringName(CategoryManagementDbProperties.ConnectionStringName)]
public interface ICategoryManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
    
    DbSet<Category> Categories { get; }
    DbSet<CategoryTree> CategoryTrees { get; }
    DbSet<CategoryTreeNodeRelation> CategoryTreeNodeRelations { get; }
}
