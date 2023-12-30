using Onibi_Pro.Application.Packages.Queries.Common;

namespace Onibi_Pro.Application.Packages.Queries.GetPackages;
public record GetPackagesDto(IReadOnlyCollection<PackageDto> Packages, long Total);