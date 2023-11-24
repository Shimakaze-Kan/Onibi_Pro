using Microsoft.AspNetCore.Http;

namespace Onibi_Pro.Shared;
public static class TokenExtractorHelper
{
    public static string? GetToken(this HttpContext context)
    {
        string? token = context.Request
            .Headers["Authorization"].FirstOrDefault()?.GetValuePartOfToken();

        if (string.IsNullOrEmpty(token))
        {
            token = context.Request.Cookies[AuthenticationKeys.CookieName].GetValuePartOfToken();
        }

        return token;
    }

    private static string? GetValuePartOfToken(this string? token)
        => token?.Split(" ")[^1];
}
