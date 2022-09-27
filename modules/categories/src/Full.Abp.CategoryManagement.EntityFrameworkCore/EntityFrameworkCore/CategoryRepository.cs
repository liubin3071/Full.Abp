using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Full.Abp.Categories;
using Full.Abp.Trees.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Full.Abp.CategoryManagement.EntityFrameworkCore;

public class CategoryRepository : EfCoreTreeRepositoryBase<ICategoryManagementDbContext,Category,CategoryTree,CategoryTreeNodeRelation,Guid>,ICategoryRepository
{
    public CategoryRepository(IDbContextProvider<ICategoryManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}