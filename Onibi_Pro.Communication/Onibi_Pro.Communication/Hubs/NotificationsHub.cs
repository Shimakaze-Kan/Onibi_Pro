using Microsoft.AspNetCore.SignalR;

using Onibi_Pro.Communication.Common;

namespace Onibi_Pro.Communication.Hubs;

public class NotificationsHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        Guid userId = UserIdProvider.GetUserId(Context.GetHttpContext());

        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Guid userId = UserIdProvider.GetUserId(Context.GetHttpContext());

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId.ToString());

        await base.OnDisconnectedAsync(exception);
    }
}
