using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Services.Authentication;
internal sealed class AccountActivationService : IAccountActivationService
{
    private const string ActivationCodePrefix = "ActivationCode_";
    private readonly TimeSpan _expireTime = TimeSpan.FromHours(1);
    private readonly ICachingService _cachingService;
    private readonly IActivateEncryptionService _guidEncryptionService;
    private readonly IUserActivationRepository _userActivationRepository;

    private static string GetKey(Guid guid)
        => $"{ActivationCodePrefix}{guid}";

    public AccountActivationService(ICachingService cachingService,
        IActivateEncryptionService guidEncryptionService,
        IUserActivationRepository userActivationRepository)
    {
        _cachingService = cachingService;
        _guidEncryptionService = guidEncryptionService;
        _userActivationRepository = userActivationRepository;
    }

    public async Task<string> CreateActivationCodeAsync(UserId userId, string email, CancellationToken cancellationToken)
    {
        var userIdValue = userId.Value;
        var code = _guidEncryptionService.EncryptData(userIdValue, email);

        await _cachingService.SetCachedDataAsync(GetKey(userIdValue), code, _expireTime, cancellationToken);

        return code;
    }

    public async Task<bool> ActivateAccountAsync(string code, CancellationToken cancellationToken)
    {
        var (userId, email, isExpired) = _guidEncryptionService.DecryptData(code);

        if (isExpired)
        {
            return false;
        }

        var storedCode = await _cachingService.GetCachedDataAsync<string?>(GetKey(userId), cancellationToken);

        if (storedCode is null)
        {
            return false;
        }

        var comparsionResult = storedCode.Equals(code);

        if (!comparsionResult)
        {
            return false;
        }

        await _userActivationRepository.ActivateAsync(email, cancellationToken);
        return true;
    }
}
