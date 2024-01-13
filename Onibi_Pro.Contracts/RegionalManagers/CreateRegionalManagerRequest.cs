namespace Onibi_Pro.Contracts.RegionalManagers;
public record CreateRegionalManagerRequest(string Email, string FirstName, string LastName, List<Guid>? RestaurantIds = null);