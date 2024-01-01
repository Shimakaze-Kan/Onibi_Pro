using Onibi_Pro.Contracts.Common;

namespace Onibi_Pro.Contracts.Shipments;
public record AcceptPackageRequest(Address Origin, Guid? SourceRestaurantId = null);