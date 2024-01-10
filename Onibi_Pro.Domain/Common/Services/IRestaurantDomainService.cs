using ErrorOr;

using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;

namespace Onibi_Pro.Domain.Common.Services;
public interface IRestaurantDomainService
{
    ErrorOr<Success> AssignManagerToRestaurant(Manager manager,
        Restaurant destinationRestaurant, Restaurant? sourceRestaurant = null);
}