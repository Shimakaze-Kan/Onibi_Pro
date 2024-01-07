using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Onibi_Pro.Communication.Models;

namespace Onibi_Pro.Communication.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly IMongoCollection<Message> _messages;

    public MessageRepository(IOptions<CommunicationDatabaseSettings> communicationDatabaseSettings, IMongoClient client)
    {
        var database = client.GetDatabase(communicationDatabaseSettings.Value.DatabaseName);

        _messages = database.GetCollection<Message>(communicationDatabaseSettings.Value.MessagesCollectionName);
    }

    public async Task InsertMessageAsync(Message message, CancellationToken cancellationToken)
    {
        await _messages.InsertOneAsync(message, cancellationToken: cancellationToken);
    }

    public async Task<List<Message>> GetReceivedMessagesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var filter = Builders<Message>.Filter.ElemMatch(x => x.Recipients, recipient => recipient.UserId == userId);
        return await _messages.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Message>> GetSentMessagesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var filter = Builders<Message>.Filter.Eq(x => x.AuthorId, userId);
        return await _messages.Find(filter).ToListAsync(cancellationToken);
    }

    // TODO need to change model
    //public async Task MarkMessageAsViewedAsync(Guid userId, string messageId, CancellationToken cancellationToken)
    //{
    //    var filter = Builders<Message>.Filter.Eq(x => x.Id, messageId) &
    //        Builders<Message>.Filter.ElemMatch(x => x.Recipients, recipient => recipient.UserId == userId);

    //    var update = Builders<Message>.Update.Set(x => x.IsViewed, true);

    //    await _messages.UpdateOneAsync(filter, update, null, cancellationToken);
    //}
}
