using static Onibi_Pro.Contracts.Menus.AddMenuItemRequest;

namespace Onibi_Pro.Contracts.Menus;
public record AddMenuItemRequest(Guid MenuId, decimal Price, string Name, List<Ingredient> Ingredients)
{
    public record Ingredient(string Name, string Unit, decimal Quantity);
}