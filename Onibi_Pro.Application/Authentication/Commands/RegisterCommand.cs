using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Authentication.Commands;
public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<Success>>;
