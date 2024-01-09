using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.PackageAggregate.Events;

namespace Onibi_Pro.Application.Packages.Events;
internal sealed class PendingRestaurantManagerApprovalEventHandler : INotificationHandler<PendingRestaurantManagerApproval>
{
    private readonly INotificationService _notificationService;
    private readonly IRestaurantDetailsService _restaurantDetailsService;

    public PendingRestaurantManagerApprovalEventHandler(INotificationService notificationService,
        IRestaurantDetailsService restaurantDetailsService)
    {
        _notificationService = notificationService;
        _restaurantDetailsService = restaurantDetailsService;
    }

    public async Task Handle(PendingRestaurantManagerApproval notification, CancellationToken cancellationToken)
    {
        var managerUserIds = await _restaurantDetailsService.GetManagerUserIds(notification.RestaurantId);
        var urgency = notification.IsUrgent ? "an urgent" : "a";
        var text = $"You have {urgency} request to accept the pick-up of products from the restaurant.";

        var notificationDto = new Notification(text, managerUserIds.ConvertAll(userId => userId.Value));

        await _notificationService.SendNotification(notificationDto, cancellationToken);
    }
}
