namespace Onibi_Pro.Contracts.RegionalManagers;
public record GetManagersResponse(Guid ManagerId, Guid RestaurantId, string FirstName, string LastName, string Email);