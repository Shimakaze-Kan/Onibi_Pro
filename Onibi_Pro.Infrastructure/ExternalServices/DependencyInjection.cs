using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Infrastructure.ExternalServices.Configurations;

namespace Onibi_Pro.Infrastructure.ExternalServices;
internal static class DependencyInjection
{
    internal static IServiceCollection AddExternalServices(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.Configure<CommunicationServiceConfiguration>(
            configurationManager.GetSection($"Services:{CommunicationServiceConfiguration.Key}"));

        services.AddHttpClient();

        services.AddTransient<INotificationService, HttpNotificationService>();

        return services;
    }
}
