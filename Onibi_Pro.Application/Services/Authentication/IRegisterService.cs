using ErrorOr;

using Onibi_Pro.Domain.UserAggregate;

namespace Onibi_Pro.Application.Services.Authentication;

public interface IRegisterService
{
    Task<ErrorOr<Success>> RegisterAsync(string firstName, string lastName, string email,
        string password, UserTypes userType, CancellationToken cancellationToken = default);
}