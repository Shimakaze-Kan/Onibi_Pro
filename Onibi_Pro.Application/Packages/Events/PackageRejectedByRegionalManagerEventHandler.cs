using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.PackageAggregate.Events;

namespace Onibi_Pro.Application.Packages.Events;
internal sealed class PackageRejectedByRegionalManagerEventHandler : INotificationHandler<PackageRejectedByRegionalManager>
{
    private readonly INotificationService _notificationService;
    private readonly IManagerDetailsService _managerDetailsService;

    public PackageRejectedByRegionalManagerEventHandler(INotificationService notificationService,
        IManagerDetailsService managerDetailsService)
    {
        _notificationService = notificationService;
        _managerDetailsService = managerDetailsService;
    }

    public async Task Handle(PackageRejectedByRegionalManager notification, CancellationToken cancellationToken)
    {
        var managerUserId = await _managerDetailsService.GetUserId(notification.ManagerId);

        var notificationDto = new Notification("The shipment was rejected by the regional manager.", [managerUserId.Value]);

        await _notificationService.SendNotification(notificationDto, cancellationToken);
    }
}
