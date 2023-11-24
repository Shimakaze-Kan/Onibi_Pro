using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onibi_Pro.Application.Common.Interfaces.Services;

namespace Onibi_Pro.Infrastructure.Caching;
internal static class DependencyInjection
{
    private const string InstanceName = "Onibi_";

    internal static IServiceCollection AddCaching(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configurationManager.GetConnectionString("RedisConnection");
            options.InstanceName = InstanceName;
        });
        services.AddScoped<ICachingService, CachingService>();

        return services;
    }
}
