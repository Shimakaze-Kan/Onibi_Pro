using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Authentication.Commands;
public sealed record ConfirmEmailCommand(string Code) : IRequest<ErrorOr<Success>>;
