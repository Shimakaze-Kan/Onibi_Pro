using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.PackageAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Commands.RejectPackage;
public record RejectPackageCommand(PackageId PackageId) : IRequest<ErrorOr<Success>>;