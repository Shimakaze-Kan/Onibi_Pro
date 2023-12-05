using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.Common.ValueObjects;
public sealed class Ingredient : ValueObject
{
    public string Name { get; private set; }
    public UnitType Unit { get; private set; }
    public decimal Quantity { get; private set; }

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

#pragma warning disable CS8618
    private Ingredient() { }
#pragma warning restore CS8618
}

public enum UnitType
{
    Grams,
    Milliliters,
    Pieces,
}