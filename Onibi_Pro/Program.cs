using Onibi_Pro;
using Onibi_Pro.Application;
using Onibi_Pro.Infrastructure;

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

            app.UseCors(options
                => options.WithOrigins("https://localhost:44406").AllowAnyMethod());

            app.UseExceptionHandler();
            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.UseSpaYarp();

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}