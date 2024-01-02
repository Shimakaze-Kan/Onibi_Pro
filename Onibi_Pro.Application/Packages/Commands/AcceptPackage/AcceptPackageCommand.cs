using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.PackageAggregate.ValueObjects;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Commands.AcceptPackage;
public record AcceptPackageCommand(PackageId PackageId, CourierId CourierId, Address Origin, RestaurantId? SourceRestaurantId = null) : IRequest<ErrorOr<Success>>;
