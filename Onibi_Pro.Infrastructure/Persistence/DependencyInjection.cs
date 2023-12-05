using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Infrastructure.Persistence.Repositories;

namespace Onibi_Pro.Infrastructure.Persistence;
internal static class DependencyInjection
{
    internal static IServiceCollection AddPersistance(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddDbContext<OnibiProDbContext>(options 
            => options.UseSqlServer(configurationManager.GetConnectionString("SqlServerConnection")));
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
