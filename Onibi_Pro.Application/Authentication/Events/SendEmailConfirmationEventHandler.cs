using MediatR;

using Microsoft.AspNetCore.Http;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Domain.UserAggregate.Events;

namespace Onibi_Pro.Application.Authentication.Events;
internal sealed class SendEmailConfirmationEventHandler : INotificationHandler<UserCreated>
{
    private readonly IEmailSender _emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccountActivationService _accountActivationService;

    public SendEmailConfirmationEventHandler(IEmailSender emailSender,
        IHttpContextAccessor httpContextAccessor,
        IAccountActivationService accountActivationService)
    {
        _emailSender = emailSender;
        _httpContextAccessor = httpContextAccessor;
        _accountActivationService = accountActivationService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        var code = await _accountActivationService.CreateActivationCodeAsync(
            notification.UserId, notification.Email, cancellationToken);

        var request = _httpContextAccessor.HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";
        var confirmationUrl = $"{baseUrl}/ConfirmEmail/{code}";

        await _emailSender.SendConfirmationEmailAsync(notification.Email, confirmationUrl, cancellationToken);
    }
}
