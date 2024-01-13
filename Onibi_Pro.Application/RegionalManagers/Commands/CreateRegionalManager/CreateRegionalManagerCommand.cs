using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.RegionalManagers.Commands.CreateRegionalManager;
public record CreateRegionalManagerCommand(string Email, string FirstName, string LastName, List<RestaurantId>? RestaurantIds = null) : IRequest<ErrorOr<Success>>;