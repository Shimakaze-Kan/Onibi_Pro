using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.RegionalManagers.Commands.UpdateRegionalManager;
public record UpdateRegionalManagerCommand(
    RegionalManagerId RegionalManagerId,
    string Email,
    string FirstName,
    string LastName,
    List<RestaurantId>? RestaurantIds = null) : IRequest<ErrorOr<Success>>;