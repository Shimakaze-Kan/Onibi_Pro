using Microsoft.AspNetCore.Builder;

namespace Onibi_Pro.Infrastructure.Authentication;

public static class AuthMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenGuardMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenGuardMiddleware>();
    }
}
