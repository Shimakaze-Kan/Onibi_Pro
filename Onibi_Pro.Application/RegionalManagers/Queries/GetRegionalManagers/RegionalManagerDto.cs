using static Onibi_Pro.Application.RegionalManagers.Queries.GetRegionalManagers.RegionalManagerDto;

namespace Onibi_Pro.Application.RegionalManagers.Queries.GetRegionalManagers;

public record RegionalManagerDto(IReadOnlyCollection<RegionalManagerItem> RegionalManagers, int TotalRecords)
{
    public record RegionalManagerItem(Guid RegionalManagerId, string FirstName,
        string LastName, string Email, int NumberOfManagers, List<Guid> RestaurantIds);
}