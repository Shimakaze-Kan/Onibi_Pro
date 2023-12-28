using static Onibi_Pro.Contracts.Identity.GetManagerDetailsResponse;

namespace Onibi_Pro.Contracts.Identity;
public record GetManagerDetailsResponse(Guid ManagerId, Guid RestaurantId,
    IReadOnlyCollection<ManagerName> SameRestaurantManagers)
{
    public record ManagerName(string FirstName, string LastName);
};
