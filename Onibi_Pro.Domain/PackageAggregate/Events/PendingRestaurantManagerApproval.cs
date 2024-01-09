using Onibi_Pro.Domain.Common.Interfaces;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.PackageAggregate.Events;
public record PendingRestaurantManagerApproval(RestaurantId RestaurantId, bool IsUrgent) : IDomainEvent;
