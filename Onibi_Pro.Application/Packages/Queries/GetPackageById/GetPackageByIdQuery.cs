using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Packages.Queries.Common;
using Onibi_Pro.Domain.PackageAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Queries.GetPackageById;
public record GetPackageByIdQuery(PackageId PackageId) : IRequest<ErrorOr<PackageDto>>;