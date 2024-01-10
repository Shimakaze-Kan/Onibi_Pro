using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.RegionalManagers.Commands.CreateManager;
public record CreateManagerCommand(string Email, string FirstName, string LastName, RestaurantId RestaurantId) : IRequest<ErrorOr<Success>>;