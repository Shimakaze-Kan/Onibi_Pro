using ErrorOr;

namespace Onibi_Pro.Application.Services.Authentication;
public interface ILoginService
{
    Task<ErrorOr<AuthenticationResult>> LoginAsync(string email,
        string password, CancellationToken cancellationToken = default);

    Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default);
}
