using ErrorOr;
using MediatR;

namespace Onibi_Pro.Application.Authentication.Commands;
public record LogoutCommand() : IRequest<ErrorOr<Success>>;
