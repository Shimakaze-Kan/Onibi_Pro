using Microsoft.AspNetCore.Identity;

using Onibi_Pro.Domain.UserAggregate;

namespace Onibi_Pro.Application.Common.Interfaces.Identity;
public interface IUserService
{
    Task<IdentityResult> CreateUserAsync(User user, string plainPassword);
    Task DeleteUserAsync(string email);
    Task<User?> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(string email, string password);
}
