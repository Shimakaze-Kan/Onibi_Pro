using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.PackageAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Commands.PickUpPackage;
public record PickUpPackageCommand(PackageId PackageId) : IRequest<ErrorOr<Success>>;
