namespace Onibi_Pro.Application.Common.Interfaces.Services;

public interface ICachingService
{
    Task<T?> GetCachedDataAsync<T>(string key);
    Task RemoveCachedDataAsync(string key);
    Task SetCachedDataAsync<T>(string key, T data, TimeSpan cacheDuration);
}
