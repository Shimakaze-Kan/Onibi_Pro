using System.Data;

namespace Onibi_Pro.Application.Services.Access;
internal interface IAccessService
{
    Task<bool> RestauranExists(Guid restaurantId, IDbConnection connection);
    Task<bool> IsUserRestaurantManager(Guid restaurantId, Guid userId, IDbConnection connection);
}
