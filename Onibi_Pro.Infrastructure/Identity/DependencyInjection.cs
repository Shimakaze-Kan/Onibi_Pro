using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Common.Interfaces.Services;

namespace Onibi_Pro.Infrastructure.Identity;
internal static class DependencyInjection
{
    internal static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
