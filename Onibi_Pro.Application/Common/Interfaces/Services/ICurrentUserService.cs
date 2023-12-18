using Onibi_Pro.Domain.UserAggregate;

namespace Onibi_Pro.Application.Common.Interfaces.Services;
public interface ICurrentUserService
{
    string Email { get; }
    string FirstName { get; }
    string LastName { get; }
    Guid UserId { get; }
    UserTypes UserType { get; }
    bool CanGetCurrentUser { get; }
}
