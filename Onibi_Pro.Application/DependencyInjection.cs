using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Onibi_Pro.Application.Common.Behaviours;
using Onibi_Pro.Application.Services.Authentication;
using System.Reflection;

namespace Onibi_Pro.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddMediatR(config 
            => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        ValidatorOptions.Global.LanguageManager.Enabled = false;

        return services;
    }
}
