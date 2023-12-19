namespace Onibi_Pro.Contracts.Identity;

public record GetWhoamiResponse(Guid UserId, string Email, string FirstName, string LastName, string UserType);