namespace Onibi_Pro.Contracts.Identity;
public record GetUsersResponse(Guid Id, string FirstName, string LastName, string Email, string UserType);