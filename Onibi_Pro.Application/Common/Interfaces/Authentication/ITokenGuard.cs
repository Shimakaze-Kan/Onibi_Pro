namespace Onibi_Pro.Application.Common.Interfaces.Authentication;
public interface ITokenGuard
{
    Task AllowTokenAsync(Guid userId, string token);
    Task DenyTokenAsync(Guid userId);
    Task<bool> IsTokenAllowedAsync(Guid userId, string token);
}