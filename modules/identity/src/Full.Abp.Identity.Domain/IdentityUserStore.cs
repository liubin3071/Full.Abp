using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace Full.Abp.Identity;

[Dependency(ReplaceServices = true)]
public class IdentityUserStore : Volo.Abp.Identity.IdentityUserStore, IUserPhoneNumberStore<IdentityUser>, ITransientDependency
{
     
    public IdentityUserStore(IIdentityUserRepository userRepository, IIdentityRoleRepository roleRepository, IGuidGenerator guidGenerator, ILogger<IdentityRoleStore> logger, ILookupNormalizer lookupNormalizer, IdentityErrorDescriber describer = null) : base(userRepository, roleRepository, guidGenerator, logger, lookupNormalizer, describer)
    {
    }
    
    public Task<IdentityUser?> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return ((IIdentityUserRepository)UserRepository).FindByPhoneNumberAsync(phoneNumber, cancellationToken: cancellationToken);
    }


}