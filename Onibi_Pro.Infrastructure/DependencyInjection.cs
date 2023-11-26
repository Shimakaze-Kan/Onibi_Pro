using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Infrastructure.Authentication;
using Onibi_Pro.Infrastructure.Caching;
using Onibi_Pro.Infrastructure.Persistence;
using Onibi_Pro.Infrastructure.ReverseProxy;
using Onibi_Pro.Infrastructure.Services;

namespace Onibi_Pro.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddCaching(configurationManager);
        services.AddAuthentication(configurationManager);
        services.AddReverseProxy(configurationManager);

        return services;
    }
}
