using System;
using System.Linq;
using System.Threading.Tasks;
using Full.Abp.Trees;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;
using Xunit.Abstractions;

namespace Full.Abp.CategoryManagement.CategoryRelations;

public abstract class CategoryRelationRepository_Tests<TStartupModule> : CategoryManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ITreeRelationRepository<CategoryRelation, Guid> _categoryRelationRepository;
    private readonly IRepository<Category, Guid> _repository;

    protected CategoryRelationRepository_Tests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        _categoryRelationRepository = GetRequiredService<ITreeRelationRepository<CategoryRelation, Guid>>();
    }

    [Fact]
    public void Inject_Test()
    {
        _categoryRelationRepository.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task Insert_Test()
    {
        await _categoryRelationRepository.EnsureParentAsync("Test", "Test", "Test", Guid.NewGuid(), null, true);
        await WithUnitOfWorkAsync(async () =>
        {
            var l = await _categoryRelationRepository.GetDescendantsAsync("Test", "Test", "Test", null);
            l.ToList().Count.ShouldBe(1);
        });
    }
}