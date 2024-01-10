using ErrorOr;

using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;

namespace Onibi_Pro.Domain.Common.Services;
internal sealed class RestaurantService : IRestaurantDomainService
{
    public ErrorOr<Success> AssignManagerToRestaurant(Manager manager, 
        Restaurant destinationRestaurant, Restaurant? sourceRestaurant = null)
    {
        if (sourceRestaurant is not null)
        {
            var unassignResult = sourceRestaurant.UnassignManager(manager);

            if (unassignResult.IsError)
            {
                return unassignResult.Errors;
            }
        }       

        var result = destinationRestaurant.AssignManager(manager.UserId);

        if (result.IsError)
        {
            return result.Errors;
        }

        return new Success();
    }
}
