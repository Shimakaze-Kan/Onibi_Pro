using Microsoft.EntityFrameworkCore;

using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories;

internal sealed class UserActivationRepository : IUserActivationRepository
{
    private readonly DbContextFactory _dbContextFactory;
    private readonly IMasterDbRepository _masterDbRepository;

    public UserActivationRepository(DbContextFactory dbContextFactory,
        IMasterDbRepository masterDbRepository)
    {
        _dbContextFactory = dbContextFactory;
        _masterDbRepository = masterDbRepository;
    }

    public async Task ActivateAsync(string email, CancellationToken cancellationToken)
    {
        var clientName = await _masterDbRepository.GetClientNameByUserEmail(email);
        var context = _dbContextFactory.CreateDbContext(clientName);

        var userSet = context.Set<User>();
        var user = await userSet.SingleAsync(x => x.Email == email, cancellationToken);

        user.ConfirmEmail();

        context.SaveChanges();
    }
}
