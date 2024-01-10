using Onibi_Pro.Domain.Common.Interfaces;

namespace Onibi_Pro.Domain.UserAggregate.Events;
public record UserUpdated(string OldEmail, string NewEmail) : IDomainEvent;
