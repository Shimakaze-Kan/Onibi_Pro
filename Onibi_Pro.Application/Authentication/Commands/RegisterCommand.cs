using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Authentication.Commands;
public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    UserTypes UserType) : IRequest<ErrorOr<UserId>>;
