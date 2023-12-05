namespace Onibi_Pro.Contracts.Menus;
public record CreateMenuResponse(Guid Id, string Name, List<MenuItemResponse> MenuItems);

public record MenuItemResponse(Guid Id, string Name, decimal Price, List<IngredientResponse> Ingredients);

public record IngredientResponse(string Name, string Unit, decimal Quantity);