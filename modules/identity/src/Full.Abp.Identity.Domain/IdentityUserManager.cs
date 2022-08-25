using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Full.Abp.Identity;

public class IdentityUserManager : Volo.Abp.Identity.IdentityUserManager, IDomainService
{
    private readonly IServiceProvider _services;

    public IdentityUserManager(IdentityUserStore store, IIdentityRoleRepository roleRepository,
        Volo.Abp.Identity.IIdentityUserRepository fullUserRepository, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<IdentityUser> passwordHasher, IEnumerable<IUserValidator<IdentityUser>> userValidators,
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<IdentityUserManager> logger,
        ICancellationTokenProvider cancellationTokenProvider, IOrganizationUnitRepository organizationUnitRepository,
        ISettingProvider settingProvider)
        : base(store, roleRepository, fullUserRepository, optionsAccessor, passwordHasher,
            userValidators, passwordValidators, keyNormalizer, errors, services, logger, cancellationTokenProvider,
            organizationUnitRepository, settingProvider)
    {
        _services = services;
    }

    private IUserPhoneNumberStore<IdentityUser>? GetPhoneNumberQueryStore(bool throwOnFail = true)
    {
        var cast = Store as IUserPhoneNumberStore<IdentityUser>;
        if (throwOnFail && cast == null)
        {
            throw new NotSupportedException("Resources.StoreNotIUserPhoneNumberStore");
        }

        return cast;
    }

    public async Task<IdentityUser?> FindByPhoneNumberAsync(string phoneNumber)
    {
        ThrowIfDisposed();
        var store = GetPhoneNumberQueryStore()!;
        if (phoneNumber == null)
        {
            throw new ArgumentNullException(nameof(phoneNumber));
        }

        var user = await store.FindByPhoneNumberAsync(phoneNumber);

        // Need to potentially check all keys
        if (user == null && Options.Stores.ProtectPersonalData)
        {
            var keyRing = _services.GetService<ILookupProtectorKeyRing>();
            var protector = _services.GetService<ILookupProtector>();
            if (keyRing != null && protector != null)
            {
                foreach (var key in keyRing.GetAllKeyIds())
                {
                    var oldKey = protector.Protect(key, phoneNumber);
                    user = await store.FindByPhoneNumberAsync(oldKey, CancellationToken);
                    if (user != null)
                    {
                        return user;
                    }
                }
            }
        }

        return user;
    }
}