using Microsoft.AspNetCore.Http;
using Onibi_Pro.Shared;
using System.Net;

namespace Onibi_Pro.Infrastructure.Authentication;
public sealed class RedirectMiddleware
{
    public static Task Handler(HttpContext context)
    {
        var isApiPath = IsApiPath(context);
        var isUnauthenticated = !IsAuthenticated(context);
        var isForbidden = IsForbidden(context);
        var isMainPageRequest = IsMainPage(context);

        if (isApiPath)
        {
            return Task.CompletedTask;
        }

        if (isUnauthenticated && !isMainPageRequest)
        {
            RemoveCookieAndRedirectToMainPage(context);
        }

        if (isForbidden)
        {
            RemoveCookieAndRedirectToMainPage(context);
        }

        return Task.CompletedTask;
    }

    private static void RemoveCookieAndRedirectToMainPage(HttpContext context)
    {
        context.Response.Cookies.Delete(AuthenticationKeys.CookieName);
        context.Response.Redirect(RouteConstants.MainPage);
    }

    private static bool IsMainPage(HttpContext context)
    {
        return context.Request.Path.Equals(RouteConstants.MainPage);
    }

    private static bool IsForbidden(HttpContext context)
    {
        return context.Response.StatusCode == (int)HttpStatusCode.Forbidden;
    }

    private static bool IsAuthenticated(HttpContext context)
    {
        return context.User.Identity?.IsAuthenticated == true;
    }

    private static bool IsApiPath(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments(RouteConstants.Api);
    }
}
