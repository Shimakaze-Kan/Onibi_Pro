using Microsoft.AspNetCore.Http;
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

    public async Task InvokeAsync(HttpContext context, ITokenGuard tokenGuard, ICurrentUserService currentUserService)
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

        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
    }
}
