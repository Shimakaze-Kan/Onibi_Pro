using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Services.Authentication;
internal interface IAccountActivationService
{
    Task<bool> ActivateAccountAsync(string code, CancellationToken cancellationToken);
    Task<string> CreateActivationCodeAsync(UserId userId, string email, CancellationToken cancellationToken);
}