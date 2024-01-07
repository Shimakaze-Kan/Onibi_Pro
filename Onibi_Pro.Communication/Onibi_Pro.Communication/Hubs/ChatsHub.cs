using Microsoft.AspNetCore.SignalR;

using Onibi_Pro.Communication.Common;
using Onibi_Pro.Communication.Models;
using Onibi_Pro.Communication.Repositories;

namespace Onibi_Pro.Communication.Hubs;

public class ChatsHub : Hub
{
    private readonly IMessageRepository _messageRepository;

    public ChatsHub(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public override async Task OnConnectedAsync()
    {
        Guid userId = HeadersProvider.GetUserId(Context.GetHttpContext());

        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Guid userId = HeadersProvider.GetUserId(Context.GetHttpContext());

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId.ToString());

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(List<MessageRecipient> recipients, string title, string text)
    {
        var senderId = HeadersProvider.GetUserId(Context.GetHttpContext());
        var senderName = HeadersProvider.GetUserName(Context.GetHttpContext());

        var sentTime = DateTime.UtcNow;

        var message = new Message
        {
            Title = title,
            Text = text,
            AuthorId = senderId,
            AuthorName = senderName,
            SentTime = sentTime,
            Recipients = recipients
        };

        await _messageRepository.InsertMessageAsync(message, CancellationToken.None);

        foreach (var recipient in recipients)
        {
            await Clients.Group(recipient.UserId.ToString()).SendAsync("ReceiveMessage", message);
        }
    }
}
