using Microsoft.Extensions.Options;
using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Common.Interfaces.Services;

namespace Onibi_Pro.Infrastructure.Authentication;

internal sealed class TokenGuard : ITokenGuard
{
    private const string TokenGuardCacheKeyPrefix = "TokenGuard_";
    private readonly ICachingService _cachingService;
    private readonly JwtTokenSettings _tokenSettings;

    private static string GetKey(Guid userId)
        => $"{TokenGuardCacheKeyPrefix}{userId}";

    public TokenGuard(ICachingService cachingService,
        IOptions<JwtTokenSettings> options)
    {
        _cachingService = cachingService;
        _tokenSettings = options.Value;
    }

    public async Task AllowTokenAsync(Guid userId, string token)
    {
        await _cachingService.SetCachedDataAsync(GetKey(userId), token,
            TimeSpan.FromMinutes(_tokenSettings.ExpirationTimeInMinutes));
    }

    public async Task<bool> IsTokenAllowedAsync(Guid userId, string token)
    {
        var cachedToken = await _cachingService.GetCachedDataAsync<string>(GetKey(userId));

        if (cachedToken is null)
        {
            return false;
        }

        if (cachedToken != token)
        {
            return false;
        }

        return true;
    }

    public async Task DenyTokenAsync(Guid userId)
    {
        await _cachingService.RemoveCachedDataAsync(GetKey(userId));
    }
}
