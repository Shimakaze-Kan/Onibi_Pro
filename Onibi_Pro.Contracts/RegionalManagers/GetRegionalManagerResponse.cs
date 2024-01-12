namespace Onibi_Pro.Contracts.RegionalManagers;
public record GetRegionalManagerResponse(Guid RegionalManagerId, string FirstName, 
    string LastName, string Email, int NumberOfManagers, List<Guid> RestaurantIds);