using System.Reflection;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Common.Behaviours;
using Onibi_Pro.Application.Services.Access;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Application.Services.CuttingConcerns;

namespace Onibi_Pro.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IAccessService, AccessService>();
        services.AddScoped<IRegisterService, RegisterService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IAssignManagerService, AssignManagerService>();
        services.AddScoped<IAccountActivationService, AccountActivationService>();
        services.AddMediatR(config
            => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        ValidatorOptions.Global.LanguageManager.Enabled = false;

        return services;
    }
}
