using Onibi_Pro.Domain.Common.Interfaces;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;

namespace Onibi_Pro.Domain.PackageAggregate.Events;
public record PendingRegionalManagerApproval(RegionalManagerId RegionalManagerId, bool IsUrgent) : IDomainEvent;
