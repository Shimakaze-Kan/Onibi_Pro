using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onibi_Pro.Application.Services.Access;
internal interface IAccessService
{
    Task<bool> RestauranExists(Guid restaurantId, IDbConnection connection);
    Task<bool> IsUserRestaurantManager(Guid restaurantId, Guid userId, IDbConnection connection);
    Task<bool> CanManagerCancelOrder(Guid managerId, Guid orderId, IDbConnection connection);
}
