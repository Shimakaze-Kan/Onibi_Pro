using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.ShipmentAggregate.ValueObjects;
public sealed class CourierId : ValueObject
{
    public Guid Value { get; }

    private CourierId(Guid id)
    {
        Value = id;
    }

    public static CourierId Create(Guid id)
    {
        return new(id);
    }

    public static CourierId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
