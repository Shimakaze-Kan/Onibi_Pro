using Microsoft.EntityFrameworkCore;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;
using Onibi_Pro.Infrastructure.Authentication.DbModels;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories;
internal sealed class UserPasswordRepository(DbContextFactory dbContextFactory) : IUserPasswordRepository
{
    private readonly DbContextFactory _dbContextFactory = dbContextFactory;

    public async Task CreatePasswordAsync(UserId userId, string password, string clientName, CancellationToken cancellationToken = default)
    {
        var context = _dbContextFactory.CreateDbContext(clientName);
        var userPasswordsSet = context.Set<UserPassword>();

        await userPasswordsSet.AddAsync(new(userId, password), cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<string> GetPasswordForUserAsync(UserId userId, string clientName, CancellationToken cancellationToken = default)
    {
        var context = _dbContextFactory.CreateDbContext(clientName);
        var userPasswordsSet = context.Set<UserPassword>();

        var userPassword = await userPasswordsSet.SingleAsync(x => x.UserId == userId, cancellationToken);
        return userPassword.Password;
    }

    public async Task UpdatePasswordAsync(UserId userId, string password, string clientName, CancellationToken cancellationToken = default)
    {
        var context = _dbContextFactory.CreateDbContext(clientName);
        var userPasswordsSet = context.Set<UserPassword>();

        var userPassword = await userPasswordsSet.SingleAsync(x => x.UserId == userId, cancellationToken);
        userPassword = userPassword with { Password = password };
        await context.SaveChangesAsync(cancellationToken);
    }
}
