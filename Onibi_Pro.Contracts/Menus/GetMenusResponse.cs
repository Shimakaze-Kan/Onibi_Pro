using static Onibi_Pro.Contracts.Menus.GetMenusResponse;

namespace Onibi_Pro.Contracts.Menus;

public record GetMenusResponse(Guid Id, string Name, IReadOnlyCollection<MenuItem> MenuItems)
{
    public record MenuItem(Guid MenuItemId, string Name,
        decimal Price, IReadOnlyCollection<Ingredient> Ingredients);

    public record Ingredient(string Name, string Unit, decimal Quantity);
};
