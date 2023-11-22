using ErrorOr;
using MediatR;
using Onibi_Pro.Application.Services.Authentication;

namespace Onibi_Pro.Application.Authentication.Commands;
public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
