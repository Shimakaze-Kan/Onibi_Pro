using ErrorOr;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository,
        ITokenGuard tokenGuard,
        ILogger<AuthenticationService> logger)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _tokenGuard = tokenGuard;
        _logger = logger;
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        User? user = _userRepository.GetByEmail(email);

        if (user is null)
        {
            _logger.LogWarning("Wrong credentials for user: {email}", email);
            return Errors.Authentication.InvalidCredentials;
        }

        if (user.Password != password)
        {
            _logger.LogWarning("Wrong credentials for user: {email}", email);
            return Errors.Authentication.InvalidCredentials;
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName, user.Email);
        _tokenGuard.AllowTokenAsync(user.Id, token, CancellationToken.None).GetAwaiter().GetResult();

        return new AuthenticationResult(user, token);
    }

    public void Logout(Guid userId)
    {
        _tokenGuard.DenyTokenAsync(userId, CancellationToken.None).GetAwaiter().GetResult();
    }

    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        User? user = _userRepository.GetByEmail(email);

        if (user is not null)
        {
            _logger.LogWarning("User already exists: {email}", email);
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

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName, user.Email);
        _tokenGuard.AllowTokenAsync(user.Id, token, CancellationToken.None).GetAwaiter().GetResult();

        return new AuthenticationResult(user, token);
    }
}
