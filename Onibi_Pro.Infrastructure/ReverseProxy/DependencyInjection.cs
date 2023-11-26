using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Onibi_Pro.Infrastructure.ReverseProxy;
internal static class DependencyInjection
{
    private const string ReverseProxyConfigurationKey = "ReverseProxy";

    public static IServiceCollection AddReverseProxy(
        this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddReverseProxy()
            .LoadFromConfig(configurationManager.GetSection(ReverseProxyConfigurationKey));

        return services;
    }
}
