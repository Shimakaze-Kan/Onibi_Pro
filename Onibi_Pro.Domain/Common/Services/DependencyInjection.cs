using Microsoft.Extensions.DependencyInjection;

namespace Onibi_Pro.Domain.Common.Services;
public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddSingleton<IRestaurantDomainService, RestaurantService>();

        return services;
    }
}
