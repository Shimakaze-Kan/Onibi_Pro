using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Shared;
using System.Net;

namespace Onibi_Pro.Infrastructure.Authentication;
internal sealed class TokenGuardMiddleware
{
    private readonly RequestDelegate _next;

    public TokenGuardMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenGuard tokenGuard,
        ICurrentUserService currentUserService, ILogger<TokenGuardMiddleware> logger)
    {
        if (!currentUserService.CanGetCurrentUser)
        {
            await _next(context);
            return;
        }

        var userId = currentUserService.UserId;
        var token = context.GetToken();

        var isTokenAllowed = await tokenGuard.IsTokenAllowedAsync(userId, token!, context.RequestAborted);

        if (isTokenAllowed)
        {
            await _next(context);
            return;
        }

        logger.LogWarning("Blocked token for user id: {userId}, IP: {ip}",
            userId, context.Connection.RemoteIpAddress);

        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
    }
}
