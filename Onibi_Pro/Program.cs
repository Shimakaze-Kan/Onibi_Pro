using Microsoft.AspNetCore.HttpOverrides;

using Onibi_Pro;
using Onibi_Pro.Application;
using Onibi_Pro.Infrastructure;
using Onibi_Pro.Infrastructure.Authentication;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            builder.Services.AddPresentation()
                            .AddApplication()
                            .AddInfrastructure(builder.Configuration);
        }

        var app = builder.Build();
        {
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Onibi API V1");
                });
            }

            //app.UseCors(options
            //    => options.WithOrigins("https://localhost:44406").AllowAnyMethod());

            app.UseExceptionHandler();

            app.UseStatusCodePages(async (context)
                => await RedirectMiddleware.Handler(context.HttpContext));
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto
            });

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");


            app.UseAuthentication();
            app.UseAuthorization();
            app.MapReverseProxy();

            app.UseTokenGuardMiddleware();

            //app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}