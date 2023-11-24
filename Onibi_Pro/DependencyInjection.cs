using ErrorOr;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Http;
using Onibi_Pro.Mapping;
using Onibi_Pro.Services;

namespace Onibi_Pro;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddCors();
        services.AddControllersWithViews();
        services.AddMapping();

        services.AddProblemDetails(options
                => options.CustomizeProblemDetails = CustomizeProblemDetails);

        services.AddRazorPages();
        services.AddSpaYarp();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

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
