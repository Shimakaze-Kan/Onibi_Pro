namespace Onibi_Pro.Contracts.Authentication;
public record LoginRequest(
    string Email,
    string Password);
