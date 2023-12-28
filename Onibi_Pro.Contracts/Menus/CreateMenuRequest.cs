using static Onibi_Pro.Contracts.Menus.CreateMenuRequest;

namespace Onibi_Pro.Contracts.Menus;
public record CreateMenuRequest(string Name, List<MenuItem> MenuItems)
{
    public record MenuItem(string Name, decimal Price, List<Ingredient> Ingredients);
    public record Ingredient(string Name, string Unit, decimal Quantity);
}
