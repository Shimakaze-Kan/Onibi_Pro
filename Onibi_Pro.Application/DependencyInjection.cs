using Microsoft.Extensions.DependencyInjection;
using Onibi_Pro.Application.Services.Authentication;

namespace Onibi_Pro.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
