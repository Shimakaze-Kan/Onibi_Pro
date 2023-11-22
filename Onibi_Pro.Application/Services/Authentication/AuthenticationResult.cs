using Onibi_Pro.Domain.Entities;

namespace Onibi_Pro.Application.Services.Authentication;
public record AuthenticationResult(
    User User,
    string Token);
