using ErrorOr;
using MediatR;
using Onibi_Pro.Application.Services.Authentication;

namespace Onibi_Pro.Application.Authentication.Queries;
public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
