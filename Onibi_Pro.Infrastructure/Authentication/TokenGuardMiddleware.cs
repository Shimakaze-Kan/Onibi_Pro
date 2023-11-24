using Microsoft.AspNetCore.Http;
using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Onibi_Pro.Infrastructure.Authentication;
internal sealed class TokenGuardMiddleware
{
    private readonly RequestDelegate _next;

    public TokenGuardMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenGuard tokenGuard)
    {
        string? token = GetToken(context);

        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
        {
            await _next(context);
            return;
        }

        if (handler.ReadToken(token) is not JwtSecurityToken tokenObject)
        {
            await _next(context);
            return;
        }

        var userId = Guid.Parse(tokenObject.Subject);

        var isTokenAllowed = await tokenGuard.IsTokenAllowedAsync(userId, token!);

        if (isTokenAllowed)
        {
            await _next(context);
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
    }

    private static string? GetToken(HttpContext context)
    {
        string? token = context.Request.Headers["Authorization"];

        if (string.IsNullOrEmpty(token))
        {
            token = context.Request.Cookies[AuthenticationKeys.CookieName];
        }

        return token;
    }
}
