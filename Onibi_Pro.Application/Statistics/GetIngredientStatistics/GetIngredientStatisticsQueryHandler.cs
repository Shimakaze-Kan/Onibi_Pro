using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Application.Statistics.GetIngredientStatistics;
internal sealed class GetIngredientStatisticsQueryHandler
    : IRequestHandler<GetIngredientStatisticsQuery, ErrorOr<IReadOnlyCollection<IngredientStatisticsDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public GetIngredientStatisticsQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<IReadOnlyCollection<IngredientStatisticsDto>>> Handle(GetIngredientStatisticsQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var result = await connection.QueryAsync<IngredientStatisticsDto>(@"
            WITH IngredientCounts AS (
                SELECT TOP 20
                    I.Name AS IngredientName,
                    SUM(I.Quantity) AS TotalQuantity
                FROM
                    Ingredients I
                INNER JOIN MenuItems MI ON I.MenuItemId = MI.MenuItemId AND I.MenuItemMenuId = MI.MenuId
                INNER JOIN OrderItem OI ON MI.MenuItemId = OI.MenuItemId
                INNER JOIN Orders O ON OI.OrderId = O.Id
                GROUP BY
                    I.Name
            )
            SELECT
                IC.IngredientName,
                IC.TotalQuantity
            FROM
                IngredientCounts IC
            ORDER BY
                IC.TotalQuantity DESC;
            ", cancellationToken);

        return result.ToList();
    }
}
