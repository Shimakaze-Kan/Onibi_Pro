using System.Data;

using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.ValueObjects;

namespace Onibi_Pro.Application.Menus.Queries.GetMenus;
internal sealed class GetMenusQueryHandler : IRequestHandler<GetMenusQuery, ErrorOr<IReadOnlyCollection<MenuDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public GetMenusQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<IReadOnlyCollection<MenuDto>>> Handle(GetMenusQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var intermediateResults = await GetIntermediateResults(connection);

        return MapIntermediateResults(intermediateResults);
    }

    private static List<MenuDto> MapIntermediateResults(List<IntermediateMenuDto> intermediateResults)
    {
        return intermediateResults.Select(intermediateMenu =>
            new MenuDto(
                intermediateMenu.MenuId,
                intermediateMenu.MenuName,
                intermediateMenu.MenuItems.Select(intermediateMenuItem =>
                    new MenuItemDto(
                        intermediateMenuItem.MenuItemId,
                        intermediateMenuItem.MenuItemName,
                        intermediateMenuItem.Price,
                        intermediateMenuItem.Ingredients.Select(intermediateIngredient =>
                            new IngredientDto(
                                intermediateIngredient.IngredientName,
                                Enum.TryParse<UnitType>(intermediateIngredient.Unit, true, out var unitType) 
                                    ? unitType : throw new InvalidOperationException("Invalid unit type."),
                                intermediateIngredient.Quantity
                            )
                        ).ToList()
                    )
                ).ToList()
            )
        ).ToList();
    }

    private static async Task<List<IntermediateMenuDto>> GetIntermediateResults(IDbConnection connection)
    {
        string query = @$"
            SELECT
                m.Id AS {nameof(IntermediateMenuDto.MenuId)},
                m.Name AS {nameof(IntermediateMenuDto.MenuName)},
                mi.MenuItemId AS {nameof(IntermediateMenuItemDto.MenuItemId)},
                mi.Name AS {nameof(IntermediateMenuItemDto.MenuItemName)},
                mi.Price AS {nameof(IntermediateMenuItemDto.Price)},
                ig.Name AS {nameof(IntermediateIngredientDto.IngredientName)},
                ig.Unit AS {nameof(IntermediateIngredientDto.Unit)},
                ig.Quantity AS {nameof(IntermediateIngredientDto.Quantity)}
            FROM dbo.Menus m
            JOIN dbo.MenuItems mi ON mi.MenuId = m.Id
            JOIN dbo.Ingredients ig ON ig.MenuItemId = mi.MenuItemId
            WHERE mi.IsDeleted = 0";

        var menuDictionary = new Dictionary<Guid, IntermediateMenuDto>();

        await connection.QueryAsync<IntermediateMenuDto, IntermediateMenuItemDto, IntermediateIngredientDto, IntermediateMenuDto>(
            query,
            (menu, menuItem, ingredient) =>
            {
                if (!menuDictionary.TryGetValue(menu.MenuId, out var menuEntry))
                {
                    menuEntry = menu;
                    menuEntry.MenuItems = [];
                    menuDictionary.Add(menuEntry.MenuId, menuEntry);
                }

                var menuItemEntry = menuEntry.MenuItems.FirstOrDefault(mi => mi.MenuItemId == menuItem.MenuItemId);
                if (menuItemEntry == null)
                {
                    menuItemEntry = menuItem;
                    menuItemEntry.Ingredients = [];
                    menuEntry.MenuItems.Add(menuItemEntry);
                }

                menuItemEntry.Ingredients.Add(ingredient);

                return menuEntry;
            },
            splitOn: $"{nameof(IntermediateMenuDto.MenuId)},{nameof(IntermediateMenuItemDto.MenuItemId)},{nameof(IntermediateIngredientDto.IngredientName)}"
        );

        return [.. menuDictionary.Values];
    }

    private class IntermediateMenuDto
    {
        public Guid MenuId { get; set; }
        public string MenuName { get; set; } = "";
        public List<IntermediateMenuItemDto> MenuItems { get; set; } = [];
    }

    private class IntermediateMenuItemDto
    {
        public Guid MenuItemId { get; set; }
        public string MenuItemName { get; set; } = "";
        public decimal Price { get; set; }
        public List<IntermediateIngredientDto> Ingredients { get; set; } = [];
    }

    private class IntermediateIngredientDto
    {
        public string IngredientName { get; set; } = "";
        public string Unit { get; set; } = "";
        public decimal Quantity { get; set; }
    }
}
