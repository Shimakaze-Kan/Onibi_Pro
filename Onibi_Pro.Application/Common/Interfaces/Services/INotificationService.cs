using Onibi_Pro.Application.Common.Models;

namespace Onibi_Pro.Application.Common.Interfaces.Services;
public interface INotificationService
{
    Task SendNotification(Notification notification, CancellationToken cancellationToken = default);
}
