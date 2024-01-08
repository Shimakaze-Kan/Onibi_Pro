using Onibi_Pro.Communication.Models;

namespace Onibi_Pro.Communication.Repositories;
public interface IMessageRepository
{
    Task<List<MessageDto>> GetReceivedMessagesAsync(Guid userId, CancellationToken cancellationToken);
    Task<List<MessageDto>> GetSentMessagesAsync(Guid userId, CancellationToken cancellationToken);
    Task InsertMessageAsync(Message message, CancellationToken cancellationToken);
    Task MarkMessageAsViewedAsync(string messageId, Guid userId, CancellationToken cancellationToken);
    Task DeleteMessageAsync(string messageId, Guid userId, CancellationToken cancellationToken);
}