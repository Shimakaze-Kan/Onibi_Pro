using Onibi_Pro.Contracts.Shipments.Common;

namespace Onibi_Pro.Contracts.Shipments;
public record GetPackagesResponse(IReadOnlyCollection<PackageItem> Packages, long Total);