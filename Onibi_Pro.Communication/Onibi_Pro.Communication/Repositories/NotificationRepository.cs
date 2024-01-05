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

    public async Task<Notification?> Create(Notification notification)
    {
        notification.Id = ObjectId.GenerateNewId().ToString();

        await _notificationCollection.InsertOneAsync(notification);

        return notification;
    }

    public async Task<Notification?> GetById(string id)
    {
        return await _notificationCollection.Find(n => n.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Notification>> GetChunk(DateTime startDateTime, DateTime? endDateTime = null)
    {
        var toDate = endDateTime ?? DateTime.UtcNow;

        var filterBuilder = Builders<Notification>.Filter;
        var dateFilter = filterBuilder.Gte(n => n.SentAt, startDateTime) & filterBuilder.Lt(n => n.SentAt, toDate);

        var isReadFilter = filterBuilder.Eq(n => n.IsRead, false);

        var filter = dateFilter & isReadFilter;

        var notifications = await _notificationCollection.Find(filter).ToListAsync();

        return notifications;
    }

    public async Task<ReplaceOneResult> Update(string id, Notification notification)
    {
        return await _notificationCollection.ReplaceOneAsync(n => n.Id == id, notification);
    }
}
