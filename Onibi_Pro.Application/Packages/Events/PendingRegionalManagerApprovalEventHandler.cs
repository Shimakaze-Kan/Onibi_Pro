using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.PackageAggregate.Events;

namespace Onibi_Pro.Application.Packages.Events;
internal sealed class PendingRegionalManagerApprovalEventHandler : INotificationHandler<PendingRegionalManagerApproval>
{
    private readonly INotificationService _notificationService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;

    public PendingRegionalManagerApprovalEventHandler(INotificationService notificationService,
        IRegionalManagerDetailsService regionalManagerDetailsService)
    {
        _notificationService = notificationService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
    }

    public async Task Handle(PendingRegionalManagerApproval notification, CancellationToken cancellationToken)
    {
        var regionalManagerUserId = await _regionalManagerDetailsService.GetUserId(notification.RegionalManagerId);
        var urgent = notification.IsUrgent ? " an urgent " : " a ";
        var text = $"You have{urgent}shipment to approve.";

        var notificationDto = new Notification(text, [regionalManagerUserId.Value]);

        await _notificationService.SendNotification(notificationDto, cancellationToken);
    }
}
