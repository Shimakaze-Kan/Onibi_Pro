using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate;

namespace Onibi_Pro.Application.Restaurants.Commands.CreateRestaurant;
public record CreateRestaurantCommand(AddressObject Address, RegionalManagerId RegionalManagerId) : IRequest<ErrorOr<Restaurant>>;