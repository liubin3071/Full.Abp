using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Full.Abp.Categories;

public class UserCategoryService : CategoryServiceBase, ITransientDependency
{
    private readonly ICurrentUser _currentUser;
    protected override string ProviderKey => _currentUser.Id.ToString();
    protected override string ProviderName => "U";

    public UserCategoryService(ICategoryServiceFactory categoryServiceFactory, ICurrentUser currentUser) : base(
        categoryServiceFactory)
    {
        _currentUser = currentUser;
    }
}