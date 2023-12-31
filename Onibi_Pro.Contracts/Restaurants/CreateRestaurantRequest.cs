using Onibi_Pro.Contracts.Common;

using static Onibi_Pro.Contracts.Restaurants.CreateRestaurantRequest;

namespace Onibi_Pro.Contracts.Restaurants;
public record CreateRestaurantRequest(Address Address, Guid RegionalManagerId);