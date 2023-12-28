using static Onibi_Pro.Contracts.Menus.CreateMenuResponse;

namespace Onibi_Pro.Contracts.Menus;
public record CreateMenuResponse(Guid Id, string Name, List<MenuItem> MenuItems)
{
    public record MenuItem(Guid Id, string Name, decimal Price, List<Ingredient> Ingredients);
    public record Ingredient(string Name, string Unit, decimal Quantity);
};
