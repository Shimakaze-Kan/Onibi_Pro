using Microsoft.AspNetCore.SignalR;

using Onibi_Pro.Communication.Exceptions;

namespace Onibi_Pro.Communication.Hubs;

public class NotificationsHub : Hub
{
    private const string ClientIdHeader = "X-ClientId";

    public override async Task OnConnectedAsync()
    {
        Guid userId = GetUserId();

        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Guid userId = GetUserId();

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId.ToString());

        await base.OnDisconnectedAsync(exception);
    }

    private Guid GetUserId()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext is null ||
            !httpContext.Request.Headers.TryGetValue(ClientIdHeader, out var clientId)
            || !Guid.TryParse(clientId, out var userId))
        {
            throw new ClientIdNotFoundException();
        }

        return userId;
    }
}
