using Onibi_Pro.Domain.Common.ValueObjects;

namespace Onibi_Pro.Application.Menus.Queries.GetMenus;

public record MenuDto(Guid Id, string Name, IReadOnlyCollection<MenuItemDto> MenuItems);

public record MenuItemDto(Guid MenuItemId, string Name, decimal Price, IReadOnlyCollection<IngredientDto> Ingredients);

public record IngredientDto(string Name, UnitType Unit, decimal Quantity);
