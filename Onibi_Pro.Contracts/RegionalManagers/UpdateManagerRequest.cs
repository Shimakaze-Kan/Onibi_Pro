namespace Onibi_Pro.Contracts.RegionalManagers;
public record UpdateManagerRequest(Guid ManagerId, string Email,
    string FirstName, string LastName, Guid RestaurantId);