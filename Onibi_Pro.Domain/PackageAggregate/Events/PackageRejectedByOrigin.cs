using Onibi_Pro.Domain.Common.Interfaces;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.PackageAggregate.Events;
public record PackageRejectedByOrigin(RegionalManagerId RegionalManagerId, ManagerId ManagerId,
    RestaurantId RestaurantId, Address RestaurantAddress) : IDomainEvent;