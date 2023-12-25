using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Infrastructure.MasterDb.Repositories;

namespace Onibi_Pro.Infrastructure.MasterDb;
internal static class DependencyInjection
{
    internal static IServiceCollection AddMasterDb(this IServiceCollection services)
    {
        services.AddScoped<IMasterDbRepository, MasterDbRepository>();

        return services;
    }
}
