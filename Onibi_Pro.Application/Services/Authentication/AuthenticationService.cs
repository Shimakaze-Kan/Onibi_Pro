using ErrorOr;
using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Entities;

namespace Onibi_Pro.Application.Services.Authentication;
internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly ITokenGuard _tokenGuard;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository,
        ITokenGuard tokenGuard)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _tokenGuard = tokenGuard;
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        User? user = _userRepository.GetByEmail(email);

        if (user is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        if (user.Password != password)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);
        _tokenGuard.AllowTokenAsync(user.Id, token).GetAwaiter().GetResult();

        return new AuthenticationResult(user, token);
    }

    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        User? user = _userRepository.GetByEmail(email);

        if (user is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        user = new()
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Password = password
        };

        _userRepository.Add(user);

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);
        _tokenGuard.AllowTokenAsync(user.Id, token).GetAwaiter().GetResult();

        return new AuthenticationResult(user, token);
    }
}
