using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Common.Interfaces.Identity;
using Onibi_Pro.Infrastructure.Persistence;

namespace Onibi_Pro.Infrastructure.Identity;
internal static class DependencyInjection
{
    internal static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<ApplicationUser>(opt =>
        {
            opt.Password.RequiredLength = 7;
            opt.Password.RequireDigit = false;
            opt.Password.RequireUppercase = false;
            opt.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<OnibiProDbContext>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
