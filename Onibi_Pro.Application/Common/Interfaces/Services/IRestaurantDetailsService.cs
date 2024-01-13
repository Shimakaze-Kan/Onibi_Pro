using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Common.Interfaces.Services;
public interface IRestaurantDetailsService
{
    Task<List<UserId>> GetManagerUserIds(RestaurantId restaurantId);
    Task<bool> AreRestaurantsAssignedToAnyRegionalManager(List<RestaurantId>? restaurants);
}
