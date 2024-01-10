namespace Onibi_Pro.Application.RegionalManagers.Queries.GetManagers;
public record ManagerDto(Guid ManagerId, Guid RestaurantId, string FirstName, string LastName, string Email);