using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Shared;

namespace Onibi_Pro.Infrastructure.Authentication;
internal static class DependencyInjection
{
    public static IServiceCollection AddAuthentication(
        this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.Configure<JwtTokenSettings>(configurationManager.GetSection(JwtTokenSettings.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ITokenGuard, TokenGuard>();
        services.AddSecurity(configurationManager);

        return services;
    }

    private static void AddSecurity(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[AuthenticationKeys.CookieName];
                            return Task.CompletedTask;
                        }
                    };

                    var jwtSettings = configurationManager.GetSection(JwtTokenSettings.SectionName).Get<JwtTokenSettings>()!;
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    };
                });

        services.AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());
    }
}
