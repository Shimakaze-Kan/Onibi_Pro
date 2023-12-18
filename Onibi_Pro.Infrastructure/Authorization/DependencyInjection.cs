using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Domain.UserAggregate;

using Onibi_Pro.Shared;

namespace Onibi_Pro.Infrastructure.Authorization;
internal static class DependencyInjection
{
    internal static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddGlobalManagerPolicy()
            .AddRegionalManagerPolicy()
            .AddManagerPolicy()
            .AddGlobalOrRegionalManagerPolicy()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());

        return services;
    }

    private static AuthorizationBuilder AddGlobalManagerPolicy(this AuthorizationBuilder builder)
        => builder.AddPolicy(AuthorizationPolicies.GlobalManagerAccess,
                policy => policy.RequireClaim(JwtKeys.UserTypeKey, UserTypes.GlobalManager.ToString().ToUpper()));

    private static AuthorizationBuilder AddRegionalManagerPolicy(this AuthorizationBuilder builder)
        => builder.AddPolicy(AuthorizationPolicies.RegionalManagerAccess,
                policy => policy.RequireClaim(JwtKeys.UserTypeKey, UserTypes.RegionalManager.ToString().ToUpper()));

    private static AuthorizationBuilder AddManagerPolicy(this AuthorizationBuilder builder)
        => builder.AddPolicy(AuthorizationPolicies.ManagerAccess,
                policy => policy.RequireClaim(JwtKeys.UserTypeKey, UserTypes.Manager.ToString().ToUpper()));

    private static AuthorizationBuilder AddGlobalOrRegionalManagerPolicy(this AuthorizationBuilder builder)
        => builder.AddPolicy(AuthorizationPolicies.GlobalOrRegionalManagerAccess,
                policy => policy.RequireAssertion(context =>
                    context.User.HasClaim(JwtKeys.UserTypeKey, UserTypes.RegionalManager.ToString().ToUpper()) ||
                    context.User.HasClaim(JwtKeys.UserTypeKey, UserTypes.GlobalManager.ToString().ToUpper())));
}
