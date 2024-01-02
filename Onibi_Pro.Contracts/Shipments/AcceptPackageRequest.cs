using Onibi_Pro.Contracts.Common;

namespace Onibi_Pro.Contracts.Shipments;
public record AcceptPackageRequest(Guid CourierId, Address Origin, Guid? SourceRestaurantId = null);