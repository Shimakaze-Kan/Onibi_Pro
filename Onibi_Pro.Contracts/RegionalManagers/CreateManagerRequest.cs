namespace Onibi_Pro.Contracts.RegionalManagers;
public record CreateManagerRequest(string Email, string FirstName, string LastName, Guid RestaurantId);