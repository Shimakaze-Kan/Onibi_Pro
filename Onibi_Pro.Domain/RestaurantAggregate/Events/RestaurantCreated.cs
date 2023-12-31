using Onibi_Pro.Domain.Common.Interfaces;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RestaurantAggregate.Events;
public record RestaurantCreated(Restaurant Restaurant, RegionalManagerId RegionalManagerId) : IDomainEvent;
