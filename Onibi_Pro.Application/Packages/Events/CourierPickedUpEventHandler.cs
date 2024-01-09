using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.PackageAggregate.Events;

namespace Onibi_Pro.Application.Packages.Events;
internal sealed class CourierPickedUpEventHandler : INotificationHandler<CourierPickedUp>
{
    private readonly INotificationService _notificationService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;
    private readonly IManagerDetailsService _managerDetailsService;

    public CourierPickedUpEventHandler(INotificationService notificationService,
        IRegionalManagerDetailsService regionalManagerDetailsService,
        IManagerDetailsService managerDetailsService)
    {
        _notificationService = notificationService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
        _managerDetailsService = managerDetailsService;
    }

    public async Task Handle(CourierPickedUp notification, CancellationToken cancellationToken)
    {
        var ingredients = string.Join(", ", notification.Ingredients.Select(ingredient => ingredient.Name));
        var text = $"Courier picked up package with: {ingredients}.";

        var regionalManagerUserId = await _regionalManagerDetailsService.GetUserId(notification.RegionalManagerId);
        var managerUserId = await _managerDetailsService.GetUserId(notification.ManagerId);

        var notificationDto = new Notification(text, [regionalManagerUserId.Value, managerUserId.Value]);

        await _notificationService.SendNotification(notificationDto, cancellationToken);
    }
}
