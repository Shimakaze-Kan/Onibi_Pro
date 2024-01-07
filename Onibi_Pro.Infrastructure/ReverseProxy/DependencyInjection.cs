using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Common.Interfaces.Services;

using Yarp.ReverseProxy.Transforms;

namespace Onibi_Pro.Infrastructure.ReverseProxy;
internal static class DependencyInjection
{
    private const string ReverseProxyConfigurationKey = "ReverseProxy";
    private const string ClientIdHeader = "X-ClientId";
    private const string ClientNameHeader = "X-ClientName";
    private const string CommunicationServicePrefix = "communication-";

    public static IServiceCollection AddReverseProxy(
        this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddReverseProxy()
            .LoadFromConfig(configurationManager.GetSection(ReverseProxyConfigurationKey))
            .AddTransforms(transform =>
            {
                if (transform.Route.ClusterId?.StartsWith(CommunicationServicePrefix) == true)
                {
                    transform.AddRequestTransform(async context =>
                    {
                        var currentUserService = context.HttpContext.RequestServices.GetService<ICurrentUserService>();

                        if (currentUserService?.CanGetCurrentUser == true)
                        {
                            var clientName = $"{currentUserService.FirstName} {currentUserService.LastName}";

                            context.ProxyRequest.Headers.Remove(ClientNameHeader);
                            context.ProxyRequest.Headers.Remove(ClientIdHeader);

                            context.ProxyRequest.Headers.Add(ClientIdHeader, currentUserService.UserId.ToString());
                            context.ProxyRequest.Headers.Add(ClientNameHeader, clientName);
                        }

                        await Task.CompletedTask;
                    });
                }
            });

        return services;
    }
}
