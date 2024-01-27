using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Application.Statistics.GetTopMenuItems;
internal sealed class GetTopMenuItemsQueryHandler : IRequestHandler<GetTopMenuItemsQuery, ErrorOr<IReadOnlyCollection<TopMenuItemsDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public GetTopMenuItemsQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<IReadOnlyCollection<TopMenuItemsDto>>> Handle(GetTopMenuItemsQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        const string query = @"
            WITH RankedMenuItems AS (
                SELECT
                    O.RestaurantId,
                    M.Name AS MenuItemName,
                    COUNT(OI.MenuItemId) AS OrdersCount,
                    ROW_NUMBER() OVER (PARTITION BY O.RestaurantId ORDER BY COUNT(OI.MenuItemId) DESC) AS Ranking
                FROM
                    Orders O
                INNER JOIN OrderItem OI ON O.Id = OI.OrderId
                INNER JOIN MenuItems M ON OI.MenuItemId = M.MenuItemId
                GROUP BY
                    O.RestaurantId, M.Name
            )
            SELECT
                R.Id AS RestaurantId,
                RM.MenuItemName,
                RM.OrdersCount
            FROM
                Restaurants R
            INNER JOIN RankedMenuItems RM ON R.Id = RM.RestaurantId
            WHERE
                RM.Ranking <= 5;";

        var result = await connection.QueryAsync<TopMenuItemsDto>(query, cancellationToken);

        return result.ToList();
    }
}
