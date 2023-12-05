using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Infrastructure.Persistence;
internal static class DependencyInjection
{
    internal static IServiceCollection AddPersistance(this IServiceCollection services)
    {
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
