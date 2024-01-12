using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Restaurants.Queries.GetRestaurantIds;
public record GetRestaurantIdsQuery : IRequest<ErrorOr<IReadOnlyCollection<Guid>>>;