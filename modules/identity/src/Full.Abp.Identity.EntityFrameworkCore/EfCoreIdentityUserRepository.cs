using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace Full.Abp.Identity.EntityFrameworkCore;

[ExposeServices(typeof(IIdentityUserRepository))]
public class EfCoreIdentityUserRepository : Volo.Abp.Identity.EntityFrameworkCore.EfCoreIdentityUserRepository, IIdentityUserRepository
{
    public EfCoreIdentityUserRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task<IdentityUser?> FindByPhoneNumberAsync(string phoneNumber,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                u => u.PhoneNumber == phoneNumber,
                GetCancellationToken(cancellationToken)
            );
    }
}