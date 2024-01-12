namespace Onibi_Pro.Application.RegionalManagers.Queries.GetRegionalManagers;
public record RegionalManagerDto(Guid RegionalManagerId, string FirstName, 
    string LastName, string Email, int NumberOfManagers, List<Guid> RestaurantIds);