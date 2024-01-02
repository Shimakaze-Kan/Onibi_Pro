using MediatR;

using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Queries.GetAddress;
public record GetAddressQuery(RestaurantId RestaurantId) : IRequest<Address>;