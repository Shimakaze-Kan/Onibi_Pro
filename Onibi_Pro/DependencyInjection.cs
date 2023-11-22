using ErrorOr;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Onibi_Pro.Http;
using Onibi_Pro.Infrastructure.Authentication;
using Onibi_Pro.Mapping;
using System.Text;

namespace Onibi_Pro;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddCors();
        services.AddControllersWithViews();
        services.AddMapping();

        services.AddProblemDetails(options
                => options.CustomizeProblemDetails = CustomizeProblemDetails);

        services.AddRazorPages();
        services.AddSpaYarp();

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

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });


        return services;
    }

    private static void CustomizeProblemDetails(ProblemDetailsContext context)
    {
        if (context.HttpContext.Items[HttpContextItemKeys.Errors] is List<Error> errors)
        {
            context.ProblemDetails.Extensions.Add("ErrorCodes", errors.Select(x => x.Code));
        }
    }
}
