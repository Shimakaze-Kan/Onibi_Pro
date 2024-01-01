using ErrorOr;

using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Services.Authentication;

public interface IRegisterService
{
    Task<ErrorOr<Success>> RegisterAsync(string firstName, string lastName, string email,
        string password, CreatorUserType currentCreatorType, CancellationToken cancellationToken = default);
}