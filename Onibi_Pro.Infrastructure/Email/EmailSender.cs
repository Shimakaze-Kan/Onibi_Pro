using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Infrastructure.Email.Configurations;

namespace Onibi_Pro.Infrastructure.Email;

internal sealed class EmailSender : IEmailSender
{
    private const string SenderName = "Onibi Pro";
    private readonly ConfirmEmailConfiguration _confirmEmailConfiguration;
    private readonly EmailConfiguration _emailConfiguration;
    private readonly ITemplateReader _templateReader;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IOptions<ConfirmEmailConfiguration> confirmEmailOptions,
        IOptions<EmailConfiguration> emailConfiguration,
        ITemplateReader templateReader,
        ILogger<EmailSender> logger)
    {
        _confirmEmailConfiguration = confirmEmailOptions.Value;
        _emailConfiguration = emailConfiguration.Value;
        _templateReader = templateReader;
        _logger = logger;
    }

    public async Task SendConfirmationEmailAsync(string receiverEmail, string confirmationLink, CancellationToken cancellationToken)
    {
        var template = await _templateReader.ReadTemplateAsync(_confirmEmailConfiguration.TemplateName, cancellationToken);

        var logo = _templateReader.ReadImage(_confirmEmailConfiguration.LogoPath);

        var emailTemplate = template.Replace("{{logo}}", logo.ContentId)
            .Replace("{{confirm_email}}", confirmationLink);

        await SendMessage(emailTemplate, _confirmEmailConfiguration.SenderEmail,
            receiverEmail, _confirmEmailConfiguration.Subject, [logo], cancellationToken);
    }

    private async Task SendMessage(string htmlBody, string senderEmail, string receiverEmail, string subject,
        List<(Stream Stream, string Filename, string ContentId)>? images = default, CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(SenderName, senderEmail));
        message.To.Add(MailboxAddress.Parse(receiverEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlBody
        };

        if (images != null)
        {
            foreach (var image in images)
            {
                var imageAttachment = await bodyBuilder.Attachments.AddAsync(image.Filename, image.Stream, cancellationToken);
                imageAttachment.ContentId = image.ContentId;
            }
        }

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(_emailConfiguration.Host, _emailConfiguration.Port, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            _logger.LogInformation("Sent an email to: {email}, subject: {subject}", receiverEmail, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while sending an email to: {email}, subject: {subject}", receiverEmail, subject);
        }
        finally
        {
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}
