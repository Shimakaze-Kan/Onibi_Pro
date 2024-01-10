using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.RegionalManagers.Commands.UpdateManager;
public record UpdateManagerCommand(ManagerId ManagerId, string Email, 
    string FirstName, string LastName, RestaurantId RestaurantId) : IRequest<ErrorOr<Success>>;
