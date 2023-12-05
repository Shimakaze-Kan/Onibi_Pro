using Onibi_Pro.Domain.Models;

namespace Onibi_Pro.Domain.Common.ValueObjects;
public sealed class Ingredient : ValueObject
{
    public string Name { get; }
    public UnitType Unit { get; }
    public decimal Quantity { get; }

    private Ingredient(string name, UnitType unitType, decimal quantity)
    {
        Name = name;
        Unit = unitType;
        Quantity = quantity;
    }

    public static Ingredient Create(string name,
        UnitType unitType, decimal quantity)
    {
        return new(name, unitType, quantity);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Unit;
        yield return Quantity;
    }
}

public enum UnitType
{
    Grams,
    Milliliters,
    Pieces,
}