using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Common.Interfaces.Services;

using Yarp.ReverseProxy.Transforms;

namespace Onibi_Pro.Infrastructure.ReverseProxy;
internal static class DependencyInjection
{
    private const string ReverseProxyConfigurationKey = "ReverseProxy";
    private const string NotificationsSignalRClusterId = "notification-signalR-cluster";
    private const string NotificationApi = "notifications-route-api";
    private const string ClientIdHeader = "X-ClientId";

    public static IServiceCollection AddReverseProxy(
        this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddReverseProxy()
            .LoadFromConfig(configurationManager.GetSection(ReverseProxyConfigurationKey))
            .AddTransforms(transform =>
            {
                if (transform.Route.ClusterId == NotificationsSignalRClusterId
                    || transform.Route.ClusterId == NotificationApi)
                {
                    transform.AddRequestTransform(async context =>
                    {
                        var currentUserService = context.HttpContext.RequestServices.GetService<ICurrentUserService>();

                        if (currentUserService?.CanGetCurrentUser == true)
                        {
                            context.ProxyRequest.Headers.Add(ClientIdHeader, currentUserService.UserId.ToString());
                        }

                        await Task.CompletedTask;
                    });
                }
            });

        return services;
    }
}
