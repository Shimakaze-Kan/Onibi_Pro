using Onibi_Pro.Domain.Models;

namespace Onibi_Pro.Domain.MenuAggregate.ValueObjects;
public sealed class MenuId : ValueObject
{
    public Guid Value { get; }

    private MenuId(Guid id)
    {
        Value = id;
    }

    public static MenuId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
