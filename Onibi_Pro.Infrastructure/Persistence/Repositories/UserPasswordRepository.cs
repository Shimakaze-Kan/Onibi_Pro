using Microsoft.EntityFrameworkCore;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;
using Onibi_Pro.Infrastructure.Authentication.DbModels;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories;
internal sealed class UserPasswordRepository(OnibiProDbContext onibiProDbContext) : IUserPasswordRepository
{
    private readonly DbSet<UserPassword> _userPasswords = onibiProDbContext.Set<UserPassword>();
    private readonly OnibiProDbContext _onibiProDbContext = onibiProDbContext;

    public async Task CreatePasswordAsync(UserId userId, string password, CancellationToken cancellationToken = default)
    {
        await _userPasswords.AddAsync(new(userId, password), cancellationToken);
        await _onibiProDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<string> GetPasswordForUserAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var userPassword = await _userPasswords.SingleAsync(x => x.UserId == userId, cancellationToken);
        return userPassword.Password;
    }

    public async Task UpdatePasswordAsync(UserId userId, string password, CancellationToken cancellationToken = default)
    {
        var userPassword = await _userPasswords.SingleAsync(x => x.UserId == userId, cancellationToken);
        userPassword = userPassword with { Password = password };
        await _onibiProDbContext.SaveChangesAsync(cancellationToken);
    }
}
