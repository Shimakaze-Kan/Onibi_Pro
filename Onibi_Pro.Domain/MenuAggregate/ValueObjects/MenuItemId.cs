using Onibi_Pro.Domain.Models;

namespace Onibi_Pro.Domain.MenuAggregate.ValueObjects;
public sealed class MenuItemId : ValueObject
{
    public Guid Value { get; }

    private MenuItemId(Guid id)
    {
        Value = id;
    }

    public static MenuItemId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
