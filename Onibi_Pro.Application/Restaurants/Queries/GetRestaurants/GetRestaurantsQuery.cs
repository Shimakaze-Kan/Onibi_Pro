using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Restaurants.Queries.GetRestaurants;
public record GetRestaurantsQuery(string Query) : IRequest<ErrorOr<IReadOnlyCollection<RestaurantDto>>>;