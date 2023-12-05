using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;
using Onibi_Pro.Domain.Models;

namespace Onibi_Pro.Domain.MenuAggregate.Entities;
public sealed class MenuItem : Entity<MenuItemId>
{
    private readonly List<Ingredient> _ingredients;

    public string Name { get; }
    public decimal Price { get; }
    public IReadOnlyList<Ingredient> Ingredients => _ingredients;

    private MenuItem(MenuItemId id, string name, decimal price, List<Ingredient>? ingredients)
        : base(id)
    {
        Name = name;
        Price = price;
        _ingredients = ingredients ?? new();
    }

    public static MenuItem Create(string name, decimal price, List<Ingredient>? ingredients = null)
    {
        return new(MenuItemId.CreateUnique(), name, price, ingredients);
    }

    public void AddIngredient(Ingredient ingredient)
    {
        _ingredients.Add(ingredient);
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        _ingredients.Remove(ingredient);
    }
}
