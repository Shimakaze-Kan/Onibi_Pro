using MongoDB.Driver;

using Onibi_Pro.Communication.Models;

namespace Onibi_Pro.Communication.Repositories;

public interface INotificationRepository
{
    Task<Notification?> CreateAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<ReplaceOneResult> UpdateAsync(string id, Notification notification, CancellationToken cancellationToken = default);
    Task<List<Notification>> GetChunkAsync(DateTime startDateTime, DateTime? endDateTime = null, CancellationToken cancellationToken = default);
    Task<Notification?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<List<NotificationDto>> GetAllForUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
