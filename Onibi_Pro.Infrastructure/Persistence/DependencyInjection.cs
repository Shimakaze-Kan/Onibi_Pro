using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Infrastructure.Persistence.Interceptors;
using Onibi_Pro.Infrastructure.Persistence.Repositories;

namespace Onibi_Pro.Infrastructure.Persistence;
internal static class DependencyInjection
{
    internal static IServiceCollection AddPersistance(this IServiceCollection services)
    {
        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<DbContextFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddRepositories();

        return services;
    }
}
