using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Identity.Queries.GetWhoami;
public record WhoamiDto(UserId UserId, string Email, string FirstName, string LastName, UserTypes UserType);
