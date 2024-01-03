using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.PackageAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Commands.ConfirmDelivery;
public record ConfirmDeliveryCommand(PackageId PackageId) : IRequest<ErrorOr<Success>>;