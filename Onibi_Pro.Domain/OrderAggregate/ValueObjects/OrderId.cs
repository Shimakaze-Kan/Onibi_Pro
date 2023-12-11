using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.OrderAggregate.ValueObjects;
public sealed class OrderId : ValueObject
{
    public Guid Value { get; }

    private OrderId(Guid id)
    {
        Value = id;
    }

    public static OrderId Create(Guid id)
    {
        return new(id);
    }

    public static OrderId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private OrderId() { }
}
