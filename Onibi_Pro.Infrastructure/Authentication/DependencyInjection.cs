using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Infrastructure.Authentication.Configurations;
using Onibi_Pro.Shared;

namespace Onibi_Pro.Infrastructure.Authentication;
internal static class DependencyInjection
{
    public static IServiceCollection AddAuthentication(
        this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.Configure<JwtTokenSettings>(configurationManager.GetSection(JwtTokenSettings.SectionName));
        services.Configure<ActivateEncryptionConfiguration>(configurationManager.GetSection(ActivateEncryptionConfiguration.Key));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ITokenGuard, TokenGuard>();
        services.AddTokenConfig(configurationManager);
        services.AddSingleton<IPasswordService, PasswordService>();
        services.AddSingleton<IActivateEncryptionService, ActivateEncryptionService>();

        return services;
    }

    private static void AddTokenConfig(this IServiceCollection services, ConfigurationManager configurationManager)
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
    }
}
