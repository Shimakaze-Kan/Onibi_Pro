namespace Onibi_Pro.Application.Common.Interfaces.Services;
public interface IEmailSender
{
    Task SendConfirmationEmailAsync(string receiverEmail, string confirmationLink, CancellationToken cancellationToken);
}
