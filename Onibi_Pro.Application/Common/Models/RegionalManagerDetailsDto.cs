namespace Onibi_Pro.Application.Common.Models;
public record RegionalManagerDetailsDto(Guid RegionalManagerId, IReadOnlyCollection<Guid> RestaurantIds, IReadOnlyCollection<Guid> ManagerIds);
