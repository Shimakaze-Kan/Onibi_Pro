using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Infrastructure.Authentication;
using OnibiAuthorization = Onibi_Pro.Infrastructure.Authorization.DependencyInjection;
using Onibi_Pro.Infrastructure.Caching;
using Onibi_Pro.Infrastructure.Persistence;
using Onibi_Pro.Infrastructure.ReverseProxy;
using Onibi_Pro.Infrastructure.Services;
using Onibi_Pro.Infrastructure.Identity;
using Onibi_Pro.Infrastructure.MasterDb;
using Microsoft.EntityFrameworkCore;
using Onibi_Pro.Infrastructure.ExternalServices;
using Onibi_Pro.Infrastructure.Email;

namespace Onibi_Pro.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddCaching(configurationManager);
        services.AddAuthentication(configurationManager);
        services.AddReverseProxy(configurationManager);
        services.AddPersistance();
        OnibiAuthorization.AddAuthorization(services);
        services.AddIdentity();
        services.AddMasterDb();
        services.AddExternalServices(configurationManager);
        services.AddEmails(configurationManager);

        // Need this for migration
        services.AddDbContext<OnibiProDbContext>(options =>
            options.UseSqlServer(configurationManager.GetConnectionString("SqlServerConnection")));

        services.AddScoped<IManagerDetailsService, ManagerDetailsService>();
        services.AddScoped<IRegionalManagerDetailsService, RegionalManagerDetailsService>();
        services.AddScoped<ICourierDetailsService, CourierDetailsService>();
        services.AddScoped<IRestaurantDetailsService, RestaurantDetailsService>();

        return services;
    }
}
