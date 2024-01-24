using Dapper;

using ErrorOr;

using Microsoft.Extensions.Logging;

using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;


namespace Onibi_Pro.Application.Services.Authentication;
internal sealed class LoginService : ILoginService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ITokenGuard _tokenGuard;
    private readonly ILogger<LoginService> _logger;
    private readonly IPasswordService _passwordService;
    private readonly IUserPasswordRepository _userPasswordRepository;
    private readonly IMasterDbRepository _masterDbRepository;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public LoginService(IJwtTokenGenerator jwtTokenGenerator,
        ITokenGuard tokenGuard,
        ILogger<LoginService> logger,
        IPasswordService passwordService,
        IUserPasswordRepository userPasswordRepository,
        IMasterDbRepository masterDbRepository,
        IDbConnectionFactory dbConnectionFactory)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _tokenGuard = tokenGuard;
        _logger = logger;
        _passwordService = passwordService;
        _userPasswordRepository = userPasswordRepository;
        _masterDbRepository = masterDbRepository;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ErrorOr<AuthenticationResult>> LoginAsync(string email, string password,
        CancellationToken cancellationToken = default)
    {
        var clientName = await _masterDbRepository.GetClientNameByUserEmail(email);

        if (string.IsNullOrEmpty(clientName))
        {
            _logger.LogWarning("User not found in master db, email: {email}", email);
            return Errors.Authentication.InvalidCredentials;
        }

        using var connection = await _dbConnectionFactory.OpenConnectionAsync(clientName);
        var user = await connection.QueryFirstOrDefaultAsync<UserDto?>(
            "SELECT TOP 1 Id, FirstName, LastName, Email, UserType, IsEmailConfirmed FROM dbo.Users WHERE Email = @Email", new { email });

        if (user is null)
        {
            _logger.LogCritical("User not found in client db: {clientName}, email: {email}", email, clientName);
            return Errors.Authentication.InvalidCredentials;
        }

        if (!user.IsEmailConfirmed)
        {
            return Errors.Authentication.EmailNotConfirmed;
        }

        var hashedPassword = await _userPasswordRepository.GetPasswordForUserAsync(UserId.Create(user.Id), clientName, cancellationToken);

        if (!_passwordService.VerifyPassword(password, hashedPassword))
        {
            _logger.LogWarning("Wrong credentials for user: {email}", email);
            return Errors.Authentication.InvalidCredentials;
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName, user.Email, user.UserType, clientName);
        await _tokenGuard.AllowTokenAsync(user.Id, token, cancellationToken);

        return new AuthenticationResult(token);
    }

    public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await _tokenGuard.DenyTokenAsync(userId, cancellationToken);
    }

    private record UserDto(Guid Id, string FirstName, string LastName, string Email, string UserType, bool IsEmailConfirmed);
}
