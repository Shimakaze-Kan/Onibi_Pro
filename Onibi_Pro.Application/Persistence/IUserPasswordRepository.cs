using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Persistence;
public interface IUserPasswordRepository
{
    Task CreatePasswordAsync(UserId userId, string password, string clientName, CancellationToken cancellationToken = default);
    Task UpdatePasswordAsync(UserId userId, string password, string clientName, CancellationToken cancellationToken = default);
    Task<string> GetPasswordForUserAsync(UserId userId, string clientName, CancellationToken cancellationToken = default);
}
