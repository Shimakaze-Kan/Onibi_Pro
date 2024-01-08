using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;

using Onibi_Pro.Communication.Models;

namespace Onibi_Pro.Communication.Repositories;

public sealed class NotificationRepository : INotificationRepository
{
    private readonly IMongoCollection<Notification> _notificationCollection;

    public NotificationRepository(
        IOptions<CommunicationDatabaseSettings> communicationDatabaseSettings, IMongoClient client)
    {
        var database = client.GetDatabase(communicationDatabaseSettings.Value.DatabaseName);

        _notificationCollection = database.GetCollection<Notification>(communicationDatabaseSettings.Value.NotificationsCollectionName);
    }

    public async Task<Notification?> CreateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        notification.Id = ObjectId.GenerateNewId().ToString();

        await _notificationCollection.InsertOneAsync(notification, cancellationToken: cancellationToken);

        return notification;
    }

    public async Task<List<NotificationDto>> GetAllForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Notification>.Filter.ElemMatch(n => n.Recipients, r => r.UserId == userId && !r.IsDeleted);
        var notifications = await _notificationCollection.Find(filter)
            .SortByDescending(n => n.SentAt).ToListAsync(cancellationToken);

        var notificationDtos = notifications.Select(n => {
            var recipientStatus = n.Recipients.FirstOrDefault(r => r.UserId == userId);
            return new NotificationDto(
                NotificationId: n.Id,
                Text: n.Text,
                Date: n.SentAt
            );
        }).ToList();

        return notificationDtos;
    }

    public async Task<Notification?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _notificationCollection.Find(n => n.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Notification>> GetChunkAsync(DateTime startDateTime,
        DateTime? endDateTime = null, CancellationToken cancellationToken = default)
    {
        var toDate = endDateTime ?? DateTime.UtcNow;

        var filterBuilder = Builders<Notification>.Filter;
        var dateFilter = filterBuilder.Gte(n => n.SentAt, startDateTime) & filterBuilder.Lt(n => n.SentAt, toDate);

        var isReadFilter = filterBuilder.Eq(n => n.IsRead, false);

        var filter = dateFilter & isReadFilter;

        var notifications = await _notificationCollection.Find(filter)
            .SortByDescending(n => n.SentAt).ToListAsync(cancellationToken);

        return notifications;
    }

    public async Task<ReplaceOneResult> UpdateAsync(string id, Notification notification, CancellationToken cancellationToken = default)
    {
        return await _notificationCollection.ReplaceOneAsync(n => n.Id == id, notification, cancellationToken: cancellationToken);
    }

    public async Task MarkAsDeletedAsync(List<string> notificationIds, Guid userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Notification>.Filter.In(x => x.Id, notificationIds) &
                     Builders<Notification>.Filter.ElemMatch(x => x.Recipients, r => r.UserId == userId);

        var update = Builders<Notification>.Update.Set("Recipients.$.IsDeleted", true);

        await _notificationCollection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
    }
}
