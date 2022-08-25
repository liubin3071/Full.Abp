using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Full.Abp.Identity;

public interface IIdentityUserRepository : Volo.Abp.Identity.IIdentityUserRepository, IBasicRepository<IdentityUser, Guid>
{
    Task<IdentityUser?> FindByPhoneNumberAsync(
        string phoneNumber,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );
}