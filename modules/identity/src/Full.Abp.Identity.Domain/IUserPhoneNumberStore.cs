using Microsoft.AspNetCore.Identity;

namespace Full.Abp.Identity;

public interface IUserPhoneNumberStore<TUser>: Microsoft.AspNetCore.Identity.IUserPhoneNumberStore<TUser> where TUser : class
{
    Task<TUser?> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
}