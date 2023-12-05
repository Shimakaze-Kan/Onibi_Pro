namespace Onibi_Pro.Contracts.Menus;
public record CreateMenuRequest(string Name, List<MenuItemRequest> MenuItems);

public record MenuItemRequest(string Name, decimal Price, List<IngredientRequest> Ingredients);

public record IngredientRequest(string Name, string Unit, decimal Quantity);
