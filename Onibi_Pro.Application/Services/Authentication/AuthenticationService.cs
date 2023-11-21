using ErrorOr;
using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Domain.Common.Errors;

namespace Onibi_Pro.Application.Services.Authentication;
internal sealed class AuthenticationService : IAuthenticationService
{
    private static List<AuthenticationResult> _authenticationResults = new();
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        var user = _authenticationResults.FirstOrDefault(x => x.Email == email);

        if (user == null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        //if (user.Token != ) password check
        //{
        //    return Errors.Authentication.InvalidCredentials;
        //}

        return _authenticationResults.First(x => x.Email == email);
    }

    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        if (_authenticationResults.FirstOrDefault(x => x.Email == email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        var token = _jwtTokenGenerator.GenerateToken(Guid.NewGuid(), firstName, lastName);
        AuthenticationResult result = new(Guid.NewGuid(), firstName, lastName, email, token);
        _authenticationResults.Add(result);

        return result;
    }
}
