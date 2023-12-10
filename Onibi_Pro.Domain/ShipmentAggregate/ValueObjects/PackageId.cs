using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.ShipmentAggregate.ValueObjects;
public sealed class PackageId : ValueObject
{
    public Guid Value { get; }

    private PackageId(Guid id)
    {
        Value = id;
    }

    public static PackageId Create(Guid id)
    {
        return new(id);
    }

    public static PackageId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
