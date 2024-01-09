using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.PackageAggregate.Events;

namespace Onibi_Pro.Application.Packages.Events;
internal sealed class PackageRejectedByOriginEventHandler : INotificationHandler<PackageRejectedByOrigin>
{
    private readonly INotificationService _notificationService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;
    private readonly IManagerDetailsService _managerDetailsService;

    public PackageRejectedByOriginEventHandler(INotificationService notificationService,
        IRegionalManagerDetailsService regionalManagerDetailsService,
        IManagerDetailsService managerDetailsService)
    {
        _notificationService = notificationService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
        _managerDetailsService = managerDetailsService;
    }

    public async Task Handle(PackageRejectedByOrigin notification, CancellationToken cancellationToken)
    {
        var regionalManagerUserId = await _regionalManagerDetailsService.GetUserId(notification.RegionalManagerId);
        var managerUserId = await _managerDetailsService.GetUserId(notification.ManagerId);
        var text = $"The shipment was rejected by the restaurant with Id {notification.RestaurantId.Value}," +
            $" located at the address: {notification.RestaurantAddress.Street}, {notification.RestaurantAddress.City}, " +
            $"{notification.RestaurantAddress.PostalCode}, {notification.RestaurantAddress.Country}";

        var notificationDto = new Notification(text, [regionalManagerUserId.Value, managerUserId.Value]);

        await _notificationService.SendNotification(notificationDto, cancellationToken);
    }
}
