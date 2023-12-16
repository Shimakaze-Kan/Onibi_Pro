using ErrorOr;

namespace Onibi_Pro.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<ErrorOr<AuthenticationResult>> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ErrorOr<AuthenticationResult>> RegisterAsync(string firstName, string lastName, string email,
        string password, CancellationToken cancellationToken = default);
}