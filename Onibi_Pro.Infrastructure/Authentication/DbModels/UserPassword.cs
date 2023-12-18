using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Authentication.DbModels;
public record UserPassword(UserId UserId, string Password);
