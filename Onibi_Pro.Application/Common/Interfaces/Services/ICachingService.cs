namespace Onibi_Pro.Application.Common.Interfaces.Services;

public interface ICachingService
{
    Task<T?> GetCachedDataAsync<T>(string key, CancellationToken cancellationToken);
    Task RemoveCachedDataAsync(string key, CancellationToken cancellationToken);
    Task SetCachedDataAsync<T>(string key, T data, TimeSpan cacheDuration, CancellationToken cancellationToken);
}
