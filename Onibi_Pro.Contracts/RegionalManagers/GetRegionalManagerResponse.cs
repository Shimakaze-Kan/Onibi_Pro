using static Onibi_Pro.Contracts.RegionalManagers.GetRegionalManagerResponse;

namespace Onibi_Pro.Contracts.RegionalManagers;

public record GetRegionalManagerResponse(IReadOnlyCollection<RegionalManagerItem> RegionalManagers, int TotalRecords)
{
    public record RegionalManagerItem(Guid RegionalManagerId, string FirstName,
        string LastName, string Email, int NumberOfManagers, List<Guid> RestaurantIds);
}