using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Application.RegionalManagers.Queries.GetRegionalManagers;
internal sealed class GetRegionalManagersQueryHandler : IRequestHandler<GetRegionalManagersQuery, ErrorOr<IReadOnlyCollection<RegionalManagerDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public GetRegionalManagersQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<IReadOnlyCollection<RegionalManagerDto>>> Handle(GetRegionalManagersQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);
        var sql = GetSqlQuery();

        var result = await connection.QueryAsync<RegionalManagerIntermediateDto>(sql,
            new { Offset = (request.PageNumber - 1) * request.PageSize, request.PageSize });
        var groupedResults = GroupResults(result);

        return groupedResults.ToList();
    }

    private static IEnumerable<RegionalManagerDto> GroupResults(IEnumerable<RegionalManagerIntermediateDto> result)
    {
        return result.GroupBy(rm => new
        {
            rm.RegionalManagerId,
            rm.FirstName,
            rm.LastName,
            rm.Email,
            rm.NumberOfManagers
        })
        .Select(group => new RegionalManagerDto(
            group.Key.RegionalManagerId,
            group.Key.FirstName,
            group.Key.LastName,
            group.Key.Email,
            group.Key.NumberOfManagers,
            group.Select(rm => rm.RestaurantId).Distinct().ToList()));
    }

    private static string GetSqlQuery()
    {
        return $@"
            WITH ManagerCountCTE AS (
                SELECT
                    rm.RegionalManagerId AS {nameof(RegionalManagerIntermediateDto.RegionalManagerId)},
                    u.FirstName AS {nameof(RegionalManagerIntermediateDto.FirstName)},
                    u.LastName AS {nameof(RegionalManagerIntermediateDto.LastName)},
                    u.Email AS {nameof(RegionalManagerIntermediateDto.Email)},
                    rmri.RestaurantId AS {nameof(RegionalManagerIntermediateDto.RestaurantId)},
                    COUNT(m.RestaurantId) OVER (PARTITION BY rm.RegionalManagerId) 
                        AS {nameof(RegionalManagerIntermediateDto.NumberOfManagers)}
                FROM
                    dbo.RegionalManagers rm
                JOIN
                    dbo.RegionalManagerRestaurantIds rmri ON rmri.RegionalManagerId = rm.RegionalManagerId
                JOIN
                    dbo.Users u ON u.Id = rm.UserId
                LEFT JOIN
                    dbo.Managers m ON m.RestaurantId = rmri.RestaurantId
                WHERE
                    rm.RegionalManagerId IN (
                        SELECT RegionalManagerId
                        FROM dbo.RegionalManagers
                        ORDER BY RegionalManagerId
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
                    )
            )
            SELECT
                RegionalManagerId,
                FirstName,
                LastName,
                Email,
                RestaurantId,
                NumberOfManagers
            FROM
                ManagerCountCTE
            ORDER BY
                RegionalManagerId;";
    }

    private class RegionalManagerIntermediateDto
    {
        public Guid RegionalManagerId { get; init; }
        public string FirstName { get; init; } = "";
        public string LastName { get; init; } = "";
        public string Email { get; init; } = "";
        public Guid RestaurantId { get; init; }
        public int NumberOfManagers { get; init; }
    }
}
