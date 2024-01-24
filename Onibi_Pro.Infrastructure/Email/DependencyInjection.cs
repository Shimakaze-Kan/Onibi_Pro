using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Infrastructure.Email.Configurations;

namespace Onibi_Pro.Infrastructure.Email;
internal static class DependencyInjection
{
    internal static IServiceCollection AddEmails(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.Configure<EmailConfiguration>(configurationManager.GetSection(EmailConfiguration.Key));
        services.Configure<ConfirmEmailConfiguration>(configurationManager.GetSection(ConfirmEmailConfiguration.Key));

        services.AddSingleton<ITemplateReader, TemplateReader>();
        services.AddScoped<IEmailSender, EmailSender>();

        return services;
    }
}
