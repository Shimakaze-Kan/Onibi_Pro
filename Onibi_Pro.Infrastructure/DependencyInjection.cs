using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Infrastructure.Authentication;
using OnibiAuthorization = Onibi_Pro.Infrastructure.Authorization.DependencyInjection;
using Onibi_Pro.Infrastructure.Caching;
using Onibi_Pro.Infrastructure.Persistence;
using Onibi_Pro.Infrastructure.ReverseProxy;
using Onibi_Pro.Infrastructure.Services;
using Onibi_Pro.Infrastructure.Identity;

namespace Onibi_Pro.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddCaching(configurationManager);
        services.AddAuthentication(configurationManager);
        services.AddReverseProxy(configurationManager);
        services.AddPersistance(configurationManager);
        OnibiAuthorization.AddAuthorization(services);
        services.AddIdentity();

        return services;
    }
}
