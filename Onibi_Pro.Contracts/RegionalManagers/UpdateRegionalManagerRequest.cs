namespace Onibi_Pro.Contracts.RegionalManagers;
public record UpdateRegionalManagerRequest(Guid RegionalManagerId,
    string Email,
    string FirstName,
    string LastName,
    List<Guid>? RestaurantIds = null);
