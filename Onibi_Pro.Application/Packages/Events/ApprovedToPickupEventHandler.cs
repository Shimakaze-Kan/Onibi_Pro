using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.PackageAggregate.Events;

namespace Onibi_Pro.Application.Packages.Events;
internal sealed class ApprovedToPickupEventHandler : INotificationHandler<ApprovedToPickup>
{
    private readonly INotificationService _notificationService;
    private readonly ICourierDetailsService _courierDetailsService;

    public ApprovedToPickupEventHandler(INotificationService notificationService,
        ICourierDetailsService courierDetailsService)
    {
        _notificationService = notificationService;
        _courierDetailsService = courierDetailsService;
    }

    public async Task Handle(ApprovedToPickup notification, CancellationToken cancellationToken)
    {
        var urgency = notification.IsUrgent ? " an urgent" : "";
        string formattedDate = notification.Until.ToString("MM/dd/yyyy");
        var text = $"You have been assigned{urgency} pickup task until {formattedDate}";

        var courierUserId = await _courierDetailsService.GetUserId(notification.CourierId);

        var notificationDto = new Notification(text, [courierUserId.Value]);
        await _notificationService.SendNotification(notificationDto, cancellationToken);
    }
}
