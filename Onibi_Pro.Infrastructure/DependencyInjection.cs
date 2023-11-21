using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Infrastructure.Authentication;
using Onibi_Pro.Infrastructure.Services;

namespace Onibi_Pro.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.Configure<JwtTokenSettings>(configurationManager.GetSection(JwtTokenSettings.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
