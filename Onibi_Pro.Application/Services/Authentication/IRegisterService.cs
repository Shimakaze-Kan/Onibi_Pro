using ErrorOr;

using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Services.Authentication;

public interface IRegisterService
{
    Task<ErrorOr<UserId>> RegisterAsync(string firstName, string lastName, string email,
        string password, CreatorUserType currentCreatorType, CancellationToken cancellationToken = default,
        bool commitTransaction = true, UserTypes? userType = default);
}