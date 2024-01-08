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

    public async Task<List<MessageDto>> GetReceivedMessagesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var filter = Builders<Message>.Filter.ElemMatch(x => x.Recipients,
            recipient => recipient.UserId == userId && !recipient.IsDeleted);
        var messages = await _messages.Find(filter).ToListAsync(cancellationToken);

        var messageDtos = messages.Select(message => new MessageDto(message, IsMessageViewed(message, userId))).ToList();
        return messageDtos;
    }

    public async Task<List<MessageDto>> GetSentMessagesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var filter = Builders<Message>.Filter.Eq(x => x.AuthorId, userId) &
                     Builders<Message>.Filter.Eq(x => x.IsDeleted, false);
        var messages = await _messages.Find(filter).ToListAsync(cancellationToken);

        var messageDtos = messages.Select(message => new MessageDto(message, false)).ToList();
        return messageDtos;
    }

    public async Task MarkMessageAsViewedAsync(string messageId, Guid userId, CancellationToken cancellationToken)
    {
        var filter = Builders<Message>.Filter.Eq(x => x.Id, messageId) &
                     Builders<Message>.Filter.ElemMatch(x => x.Recipients,
                     recipient => recipient.UserId == userId && !recipient.IsDeleted);
        var update = Builders<Message>.Update.Set("Recipients.$.IsViewed", true);

        await _messages.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task DeleteMessageAsync(string messageId, Guid userId, CancellationToken cancellationToken)
    {
        var filter = Builders<Message>.Filter.Eq(x => x.Id, messageId);
        var message = await _messages.Find(filter).FirstOrDefaultAsync(cancellationToken);

        if (message is null)
        {
            return;
        }

        var recipientFilter = Builders<Message>.Filter.Eq(x => x.Id, messageId) &
                             Builders<Message>.Filter.ElemMatch(x => x.Recipients, r => r.UserId == userId);
        var recipientUpdate = Builders<Message>.Update.Set("Recipients.$.IsDeleted", true);
        await _messages.UpdateOneAsync(recipientFilter, recipientUpdate, cancellationToken: cancellationToken);

        var update = Builders<Message>.Update.Set(x => x.IsDeleted, true);
        await _messages.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    private static bool IsMessageViewed(Message message, Guid userId)
    {
        var recipient = message.Recipients.FirstOrDefault(r => r.UserId == userId);
        return recipient != null && recipient.IsViewed;
    }
}
