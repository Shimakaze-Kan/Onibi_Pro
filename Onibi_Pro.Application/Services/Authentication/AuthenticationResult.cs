using Onibi_Pro.Domain.UserAggregate;

namespace Onibi_Pro.Application.Services.Authentication;
public record AuthenticationResult(
    User User,
    string Token);
