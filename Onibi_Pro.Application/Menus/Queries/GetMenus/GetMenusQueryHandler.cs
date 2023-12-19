using System.Data;
using System.Text.Json;

using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Application.Menus.Queries.GetMenus;
internal sealed class GetMenusQueryHandler : IRequestHandler<GetMenusQuery, ErrorOr<IReadOnlyCollection<MenuDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetMenusQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ErrorOr<IReadOnlyCollection<MenuDto>>> Handle(GetMenusQuery request, CancellationToken cancellationToken)
    {
        // TODO check role
        using var connection = await _dbConnectionFactory.OpenConnectionAsync();
        var intermediateResults = await GetIntermediateResults(connection);

        return MapToResultDto(intermediateResults);
    }

    private static List<MenuDto> MapToResultDto(IEnumerable<IntermediateMenuDto> intermediateResults)
    {
        List<MenuDto> menuList = [];

        foreach (var entry in intermediateResults)
        {
            List<MenuItemDto> menuItems = [];

            foreach (var item in entry.MenuItems)
            {
                var ingredients = JsonSerializer.Deserialize<List<IngredientDto>>(item.Ingredients) ?? [];
                var menuItem = new MenuItemDto(item.MenuItemId, item.MenuItemName, item.Price, ingredients);
                menuItems.Add(menuItem);
            }

            var menu = new MenuDto(entry.Id, entry.Name, menuItems);
            menuList.Add(menu);
        }

        return menuList;
    }

    private static async Task<IEnumerable<IntermediateMenuDto>> GetIntermediateResults(IDbConnection connection)
    {
        string query = @"
            SELECT 
                m.Id AS Id,
                m.Name,
                mi.MenuId,
                mi.MenuItemId,
                mi.Name AS MenuItemName,
                mi.Price,
                mi.Ingredients
            FROM Menus m
            LEFT JOIN MenuItems mi ON m.Id = mi.MenuId";

        var menuDictionary = new Dictionary<Guid, IntermediateMenuDto>();

        await connection.QueryAsync<IntermediateMenuDto, IntermediateMenuItemDto, IntermediateMenuDto>(
            query, (menu, menuItem) =>
            {
                if (!menuDictionary.TryGetValue(menu.Id, out var menuEntry))
                {
                    menuEntry = menu;
                    menuDictionary.Add(menuEntry.Id, menuEntry);
                }

                if (menuItem != null)
                {
                    menuEntry.MenuItems.Add(menuItem);
                }

                return menuEntry;
            },
            splitOn: "Id,MenuId"
        );

        return menuDictionary.Values;
    }
}
