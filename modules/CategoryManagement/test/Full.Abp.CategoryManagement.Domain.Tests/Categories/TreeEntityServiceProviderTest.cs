using System;
using System.Linq;
using System.Threading.Tasks;
using Full.Abp.Trees;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Full.Abp.CategoryManagement.Categories;

public class TreeEntityServiceProviderTest : CategoryManagementDomainTestBase
{
    private ITreeEntityService<Category, Guid> CategoryService { get; }

    public TreeEntityServiceProviderTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        var treeEntityServiceProvider =
            ServiceProvider.GetRequiredService<ITreeEntityServiceFactory<Category, CategoryRelation, Guid>>();
        CategoryService = treeEntityServiceProvider.Create( "Test", "Test");
    }

    [Fact]
    public void Inject_Test()
    {
        ServiceProvider.GetService<ITreeEntityServiceFactory<Category, CategoryRelation, Guid>>().ShouldNotBeNull();
    }

    [Fact]
    public async Task Create_Test()
    {
        var root1 = new Category() { Name = "root1" };
        var child1 = new Category() { Name = "child1" };
        var child11 = new Category() { Name = "child11" };
        var child2 = new Category() { Name = "child2" };
        await CategoryService.CreateAsync("Test",root1);
        await CategoryService.CreateAsync("Test",child1, root1.Id);
        await CategoryService.CreateAsync("Test",child11, child1.Id);
        await CategoryService.CreateAsync("Test",child2, root1.Id);

        await WithUnitOfWorkAsync(async () =>
        {
            var all = await CategoryService.GetDescendantsAsync("Test",null);
            all.Count().ShouldBe(4);

            (await CategoryService.GetParentAsync("Test",root1.Id)).ShouldBeNull();
            (await CategoryService.GetParentAsync("Test",child1.Id))!.Id.ShouldBe(root1.Id);
            (await CategoryService.GetParentAsync("Test",child2.Id))!.Id.ShouldBe(root1.Id);
            (await CategoryService.GetParentAsync("Test",child11.Id))!.Id.ShouldBe(child1.Id);
        });
    }

    [Fact]
    public async Task EnsureParent_Test()
    {
        var root1 = new Category() { Name = "root1" };
        var child1 = new Category() { Name = "child1" };
        var child11 = new Category() { Name = "child11" };
        var child2 = new Category() { Name = "child2" };
        await CategoryService.CreateAsync("Test",root1);
        await CategoryService.CreateAsync("Test",child1, root1.Id);
        await CategoryService.CreateAsync("Test",child11, child1.Id);
        await CategoryService.CreateAsync("Test",child2, root1.Id);

        await CategoryService.EnsureParentAsync("Test",child2.Id, child11.Id);
        await CategoryService.EnsureParentAsync("Test",child1.Id, null);

        await WithUnitOfWorkAsync(async () =>
        {
            var all = await CategoryService.GetDescendantsAsync("Test",null);
            all.Count().ShouldBe(4);

            (await CategoryService.GetParentAsync("Test",root1.Id)).ShouldBeNull();
            (await CategoryService.GetParentAsync("Test",child1.Id)).ShouldBeNull();
            (await CategoryService.GetParentAsync("Test",child11.Id))!.Id.ShouldBe(child1.Id);
            (await CategoryService.GetParentAsync("Test",child2.Id))!.Id.ShouldBe(child11.Id);
        });
    }

    [Fact]
    public void EnsureParent_ShouldThrow_ParentNotfound_Test()
    {
        var func = async () => await CategoryService.EnsureParentAsync("Test",Guid.NewGuid(), Guid.NewGuid());
        func.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public async Task EnsureParent_ShouldThrow_ParentInvalid_Test()
    {
        var root1 = new Category() { Name = "root1" };
        var child1 = new Category() { Name = "child1" };
        var child11 = new Category() { Name = "child11" };
        await CategoryService.CreateAsync("Test",root1);
        await CategoryService.CreateAsync("Test",child1, root1.Id);
        await CategoryService.CreateAsync("Test",child11, child1.Id);

        var func = async () => await CategoryService.EnsureParentAsync("Test",child1.Id, child11.Id);
        func.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public async Task GetAncestors_Test()
    {
        var root1 = new Category() { Name = "root1" };
        var child1 = new Category() { Name = "child1" };
        var child11 = new Category() { Name = "child11" };
        await CategoryService.CreateAsync("Test",root1);
        await CategoryService.CreateAsync("Test",child1, root1.Id);
        await CategoryService.CreateAsync("Test",child11, child1.Id);
        await WithUnitOfWorkAsync(async () =>
        {
            var ancestors = (await CategoryService.GetAncestorsAsync("Test",child11.Id)).ToList();
            ancestors.Count.ShouldBe(2);

            ancestors[0].Id.ShouldBe(root1.Id);
            ancestors[1].Id.ShouldBe(child1.Id);
        });
    }
    
    
    [Fact]
    public async Task GetTree_Test()
    {
        var root1 = new Category() { Name = "root1" };
        var child1 = new Category() { Name = "child1" };
        var child11 = new Category() { Name = "child11" };
        var child2 = new Category() { Name = "child2", Sequence = 1};
        await CategoryService.CreateAsync("Test",root1);
        await CategoryService.CreateAsync("Test",child1, root1.Id);
        await CategoryService.CreateAsync("Test",child11, child1.Id);
        await CategoryService.CreateAsync("Test",child2, root1.Id);

        await WithUnitOfWorkAsync(async () =>
        {
            var tree = (await CategoryService.GetTreeAsync("Test",null))
                .TreeOrderBy(c => c.Value.Sequence)
                .ToList()
                ;

            tree[0].Value.Id.ShouldBe(root1.Id);
            tree[0].Children.ToList()[0].Value.Id.ShouldBe(child1.Id);
            tree[0].Children.ToList()[1].Value.Id.ShouldBe(child2.Id);
            tree[0].Children.ToList()[0].Children.ToList()[0].Value.Id.ShouldBe(child11.Id);
        });
    }
}