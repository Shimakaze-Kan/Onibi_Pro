using ErrorOr;
using Microsoft.AspNetCore.Diagnostics;
using Onibi_Pro.Application;
using Onibi_Pro.Http;
using Onibi_Pro.Infrastructure;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            builder.Services.AddControllersWithViews();
            builder.Services.AddApplication()
                            .AddInfrastructure(builder.Configuration);

            builder.Services.AddProblemDetails(options 
                => options.CustomizeProblemDetails = CustomizeProblemDetails);
        }

        var app = builder.Build();
        {
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseExceptionHandler();
            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }

    private static void CustomizeProblemDetails(ProblemDetailsContext context)
    {
        if (context.HttpContext.Items[HttpContextItemKeys.Errors] is List<Error> errors)
        {
            context.ProblemDetails.Extensions.Add("ErrorCodes", errors.Select(x => x.Code));
        }
    }
}