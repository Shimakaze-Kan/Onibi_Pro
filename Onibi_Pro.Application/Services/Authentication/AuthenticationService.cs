using ErrorOr;

using Microsoft.Extensions.Logging;

using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.UserAggregate;

using User = Onibi_Pro.Domain.UserAggregate.User;

namespace Onibi_Pro.Application.Services.Authentication;
internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ITokenGuard _tokenGuard;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IUserPasswordRepository _userPasswordRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator,
        ITokenGuard tokenGuard,
        ILogger<AuthenticationService> logger,
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        IUserPasswordRepository userPasswordRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _tokenGuard = tokenGuard;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _userPasswordRepository = userPasswordRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> LoginAsync(string email,
        string password, CancellationToken cancellationToken = default)
    {
        User? user = await GetUserByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("Wrong credentials for user: {email}", email);
            return Errors.Authentication.InvalidCredentials;
        }

        var hashedPassword = await _userPasswordRepository.GetPasswordForUserAsync(user.Id, cancellationToken);

        if (!_passwordService.VerifyPassword(password, hashedPassword))
        {
            _logger.LogWarning("Wrong credentials for user: {email}", email);
            return Errors.Authentication.InvalidCredentials;
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id.Value,
            user.FirstName, user.LastName, user.Email, user.UserType.ToString());
        await _tokenGuard.AllowTokenAsync(user.Id.Value, token, cancellationToken);

        return new AuthenticationResult(user, token);
    }

    public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await _tokenGuard.DenyTokenAsync(userId, cancellationToken);
    }

    public async Task<ErrorOr<AuthenticationResult>> RegisterAsync(string firstName, string lastName,
        string email, string password, UserTypes userType, CancellationToken cancellationToken = default)
    {
        User? user = await GetUserByEmailAsync(email, cancellationToken);

        if (user is not null)
        {
            _logger.LogWarning("User already exists: {email}", email);
            return Errors.User.DuplicateEmail;
        }

        var hashedPassword = _passwordService.HashPassword(password);

        user = User.CreateUnique(firstName, lastName, email, hashedPassword, userType);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogCritical(ex, "Error while creating user.");

            return Error.Unexpected();
        }

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user.Id.Value,
            user.FirstName, user.LastName, user.Email, user.UserType.ToString());
        await _tokenGuard.AllowTokenAsync(user.Id.Value, token, cancellationToken);

        return new AuthenticationResult(user, token);
    }

    private async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _unitOfWork.UserRepository.GetAsync(user => user.Email == email, cancellationToken);
    }
}
