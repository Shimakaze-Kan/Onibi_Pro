using Microsoft.AspNetCore.Identity;

using Onibi_Pro.Application.Common.Interfaces.Identity;
using Onibi_Pro.Domain.UserAggregate;

namespace Onibi_Pro.Infrastructure.Identity;
internal sealed class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> CheckPasswordAsync(string email, string password)
    {
        var applicationUser = await _userManager.FindByEmailAsync(email);
        return await _userManager.CheckPasswordAsync(applicationUser!, password);
    }

    public async Task<IdentityResult> CreateUserAsync(User user, string plainPassword)
    {
        var applicationUser = new ApplicationUser(user);
        return await _userManager.CreateAsync(applicationUser, plainPassword);
    }

    public Task DeleteUserAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        var applicationUser = await _userManager.FindByEmailAsync(email);
        return applicationUser?.ToDomainUser();
    }
}
