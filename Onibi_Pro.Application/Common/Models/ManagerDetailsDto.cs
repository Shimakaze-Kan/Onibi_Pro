namespace Onibi_Pro.Application.Common.Models;
public record ManagerDetailsDto(Guid ManagerId, Guid RestaurantId, Guid RegionalManagerId,
    IReadOnlyCollection<ManagerNames> SameRestaurantManagers);

public record ManagerNames(string FirstName, string LastName);