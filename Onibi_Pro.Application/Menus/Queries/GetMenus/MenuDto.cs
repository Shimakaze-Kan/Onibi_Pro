using Onibi_Pro.Domain.Common.ValueObjects;

namespace Onibi_Pro.Application.Menus.Queries.GetMenus;
internal class IntermediateMenuDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<IntermediateMenuItemDto> MenuItems { get; set; } = [];
}

internal record IntermediateMenuItemDto(Guid MenuId, Guid MenuItemId, string MenuItemName, decimal Price, string Ingredients);

public record MenuDto(Guid Id, string Name, IReadOnlyCollection<MenuItemDto> MenuItems);

public record MenuItemDto(Guid MenuItemId, string Name, decimal Price, IReadOnlyCollection<IngredientDto> Ingredients);

public record IngredientDto(string Name, UnitType Unit, decimal Quantity);
