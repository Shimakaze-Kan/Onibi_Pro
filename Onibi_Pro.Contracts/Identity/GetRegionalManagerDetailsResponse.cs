namespace Onibi_Pro.Contracts.Identity;
public record GetRegionalManagerDetailsResponse(Guid RegionalManagerId, IReadOnlyCollection<Guid> RestaurantIds, IReadOnlyCollection<Guid> ManagerIds);
