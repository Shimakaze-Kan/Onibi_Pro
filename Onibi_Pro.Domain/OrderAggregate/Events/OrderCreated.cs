using Onibi_Pro.Domain.Common.Interfaces;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.OrderAggregate.Events;
public record OrderCreated(OrderId OrderId, RestaurantId RestaurantId) : IDomainEvent;
