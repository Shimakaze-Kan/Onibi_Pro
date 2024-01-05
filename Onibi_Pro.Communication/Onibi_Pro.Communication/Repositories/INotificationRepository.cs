using MongoDB.Driver;

using Onibi_Pro.Communication.Models;

namespace Onibi_Pro.Communication.Repositories;

public interface INotificationRepository
{
    Task<Notification?> Create(Notification notification);
    Task<ReplaceOneResult> Update(string id, Notification notification);
    Task<List<Notification>> GetChunk(DateTime startDateTime, DateTime? endDateTime = null);
    Task<Notification?> GetById(string id);
}
