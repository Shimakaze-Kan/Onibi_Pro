﻿using Microsoft.Extensions.Caching.Distributed;
using Onibi_Pro.Application.Common.Interfaces.Services;
using System.Text.Json;

namespace Onibi_Pro.Infrastructure.Caching;

internal sealed class CachingService : ICachingService
{
    private readonly IDistributedCache _distributedCache;

    public CachingService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetCachedDataAsync<T>(string key)
    {
        var jsonData = await _distributedCache.GetStringAsync(key);

        if (jsonData == null)
            return default;

        return JsonSerializer.Deserialize<T>(jsonData);
    }

    public async Task SetCachedDataAsync<T>(string key, T data, TimeSpan cacheDuration)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration
        };

        var jsonData = JsonSerializer.Serialize(data);
        await _distributedCache.SetStringAsync(key, jsonData, options);
    }

    public async Task RemoveCachedDataAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }
}
