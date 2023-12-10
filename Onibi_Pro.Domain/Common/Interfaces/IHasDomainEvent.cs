namespace Onibi_Pro.Domain.Common.Interfaces;
public interface IHasDomainEvent
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    public void ClearDomainEvents();
}
