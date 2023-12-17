using ErrorOr;

using Microsoft.Extensions.Logging;

using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Common.Interfaces.Identity;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.UserAggregate;

namespace Onibi_Pro.Application.Services.Authentication;
internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ITokenGuard _tokenGuard;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator,
        ITokenGuard tokenGuard,
        ILogger<AuthenticationService> logger,
        IUnitOfWork unitOfWork,
        IUserService userService)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _tokenGuard = tokenGuard;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<ErrorOr<AuthenticationResult>> LoginAsync(string email,
        string password, CancellationToken cancellationToken = default)
    {
        User? user = await _userService.FindByEmailAsync(email);

        if (user is null)
        {
            _logger.LogWarning("Wrong credentials for user: {email}", email);
            return Errors.Authentication.InvalidCredentials;
        }

        if (!await _userService.CheckPasswordAsync(email, password))
        {
            _logger.LogWarning("Wrong credentials for user: {email}", email);
            return Errors.Authentication.InvalidCredentials;
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id.Value, user.FirstName, user.LastName, user.Email);
        await _tokenGuard.AllowTokenAsync(user.Id.Value, token, cancellationToken);

        return new AuthenticationResult(user, token);
    }

    public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await _tokenGuard.DenyTokenAsync(userId, cancellationToken);
    }

    public async Task<ErrorOr<AuthenticationResult>> RegisterAsync(string firstName, string lastName,
        string email, string password, CancellationToken cancellationToken = default)
    {
        var user = User.CreateUnique(firstName, lastName, email, UserTypes.Manager);

        var identityResult = await _userService.CreateUserAsync(user, password);

        if(!identityResult.Succeeded)
        {
            return identityResult.Errors.Select(error => Error.Validation(error.Code, error.Description)).ToList();
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id.Value, user.FirstName, user.LastName, user.Email);
        await _tokenGuard.AllowTokenAsync(user.Id.Value, token, cancellationToken);

        return new AuthenticationResult(user, token);
    }
}
