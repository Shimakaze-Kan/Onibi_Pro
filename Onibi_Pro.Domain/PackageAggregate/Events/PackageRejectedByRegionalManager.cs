using Onibi_Pro.Domain.Common.Interfaces;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.PackageAggregate.Events;
public record PackageRejectedByRegionalManager(ManagerId ManagerId) : IDomainEvent;
