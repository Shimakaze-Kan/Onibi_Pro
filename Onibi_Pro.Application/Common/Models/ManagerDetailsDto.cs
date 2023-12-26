namespace Onibi_Pro.Application.Common.Models;
public record ManagerDetailsDto(Guid ManagerId, Guid RestaurantId,
    IReadOnlyCollection<ManagerNames> SameRestaurantManagers);

public record ManagerNames(string FirstName, string LastName);