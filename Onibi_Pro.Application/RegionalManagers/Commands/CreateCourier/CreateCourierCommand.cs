using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.RegionalManagers.Commands.CreateCourier;
public record CreateCourierCommand(string Phone, UserId UserId) : IRequest<ErrorOr<Success>>;