namespace Onibi_Pro.Contracts.Identity;
public record GetManagerDetailsResponse(Guid RestaurantId,
    IReadOnlyCollection<GetManagerDetailsManagerNamesResponse> SameRestaurantManagers);

public record GetManagerDetailsManagerNamesResponse(string FirstName, string LastName);