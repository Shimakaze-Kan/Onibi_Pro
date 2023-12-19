using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Restaurants.Commands.AssignManager;
public record AssignManagerCommand(Guid RestaurantId, Guid UserId) : IRequest<ErrorOr<Unit>>;