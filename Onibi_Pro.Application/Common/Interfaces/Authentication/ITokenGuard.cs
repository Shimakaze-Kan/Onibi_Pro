namespace Onibi_Pro.Application.Common.Interfaces.Authentication;
public interface ITokenGuard
{
    Task AllowTokenAsync(Guid userId, string token, CancellationToken cancellationToken);
    Task DenyTokenAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> IsTokenAllowedAsync(Guid userId, string token, CancellationToken cancellationToken);
}