using Onibi_Pro.Communication.Exceptions;

namespace Onibi_Pro.Communication.Common;

public class UserIdProvider
{
    public static Guid GetUserId(HttpContext? context)
    {
        if (context is null ||
            !context.Request.Headers.TryGetValue(HeaderKeys.ClientId, out var clientId)
            || !Guid.TryParse(clientId, out var userId))
        {
            throw new ClientIdNotFoundException();
        }

        return userId;
    }
}
