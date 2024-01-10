using ErrorOr;

using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Services.CuttingConcerns;
internal interface IAssignManagerService
{
    Task<ErrorOr<Success>> AssignToRestaurant(RestaurantId restaurantId, UserId userId, CancellationToken cancellationToken = default);
}