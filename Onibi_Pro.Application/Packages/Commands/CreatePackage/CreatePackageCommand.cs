using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.PackageAggregate;

namespace Onibi_Pro.Application.Packages.Commands.CreatePackage;
public record CreatePackageCommand(IReadOnlyCollection<Ingredient> Ingredients, DateTime Until, bool IsUrgent, string Message) : IRequest<ErrorOr<Package>>;
