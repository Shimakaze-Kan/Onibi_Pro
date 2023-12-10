namespace Onibi_Pro.Contracts.Menus;

public record GetMenusResponse(Guid Id, string Name, IReadOnlyCollection<GetMenuItemResponse> MenuItems);

public record GetMenuItemResponse(Guid MenuItemId, string Name,
    decimal Price, IReadOnlyCollection<GetIngredientResponse> Ingredients);

public record GetIngredientResponse(string Name, string Unit, decimal Quantity);