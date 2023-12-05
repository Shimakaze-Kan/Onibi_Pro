using Onibi_Pro.Domain.Common.Models;

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

    public static MenuItemId Create(Guid id)
    {
        return new(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
