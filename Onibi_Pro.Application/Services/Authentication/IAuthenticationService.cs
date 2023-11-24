using ErrorOr;

namespace Onibi_Pro.Application.Services.Authentication;

public interface IAuthenticationService
{
    ErrorOr<AuthenticationResult> Login(string email, string password);
    void Logout(Guid userId);
    ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password);
}