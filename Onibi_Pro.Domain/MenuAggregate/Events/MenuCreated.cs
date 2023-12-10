using Onibi_Pro.Domain.Common.Interfaces;

namespace Onibi_Pro.Domain.MenuAggregate.Events;
public sealed record MenuCreated(Menu Menu) : IDomainEvent;
