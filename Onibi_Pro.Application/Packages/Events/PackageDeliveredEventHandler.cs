using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.PackageAggregate.Events;

namespace Onibi_Pro.Application.Packages.Events;
internal sealed class PackageDeliveredEventHandler : INotificationHandler<PackageDelivered>
{
    private readonly INotificationService _notificationService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;

    public PackageDeliveredEventHandler(INotificationService notificationService,
        IRegionalManagerDetailsService regionalManagerDetailsService)
    {
        _notificationService = notificationService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
    }

    public async Task Handle(PackageDelivered notification, CancellationToken cancellationToken)
    {
        var regionalManagerUserId = await _regionalManagerDetailsService.GetUserId(notification.RegionalManagerId);

        var notificationDto = new Notification("One of the shipments reached its destination.", [regionalManagerUserId.Value]);

        await _notificationService.SendNotification(notificationDto, cancellationToken);
    }
}
