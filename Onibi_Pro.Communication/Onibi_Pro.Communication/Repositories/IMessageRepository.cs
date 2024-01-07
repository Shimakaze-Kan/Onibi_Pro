using Onibi_Pro.Communication.Models;

namespace Onibi_Pro.Communication.Repositories;
public interface IMessageRepository
{
    Task<List<Message>> GetReceivedMessagesAsync(Guid userId, CancellationToken cancellationToken);
    Task<List<Message>> GetSentMessagesAsync(Guid userId, CancellationToken cancellationToken);
    Task InsertMessageAsync(Message message, CancellationToken cancellationToken);
}