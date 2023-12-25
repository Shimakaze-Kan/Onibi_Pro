using Onibi_Pro.Domain.Common.Interfaces;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Domain.UserAggregate.Events;
public sealed record UserCreated(UserId UserId, string Email, string HashedPassword) : IDomainEvent;
