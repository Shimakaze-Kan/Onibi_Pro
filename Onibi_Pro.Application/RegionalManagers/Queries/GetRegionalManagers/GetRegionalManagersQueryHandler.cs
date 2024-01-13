using System.Data;

using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;

using static Onibi_Pro.Application.RegionalManagers.Queries.GetRegionalManagers.RegionalManagerDto;

namespace Onibi_Pro.Application.RegionalManagers.Queries.GetRegionalManagers;
internal sealed class GetRegionalManagersQueryHandler 
    : IRequestHandler<GetRegionalManagersQuery, ErrorOr<RegionalManagerDto>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public GetRegionalManagersQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<RegionalManagerDto>> Handle(
        GetRegionalManagersQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);
        var sql = GetSqlQuery();

        var result = await connection.QueryAsync<RegionalManagerIntermediateDto>(sql,
            new
            {
                Offset = (request.PageNumber - 1) * request.PageSize + 1,
                request.PageSize,
                RegionalManagerIdFilter = FormatFilter(request.RegionalManagerIdFilter),
                FirstNameFilter = FormatFilter(request.FirstNameFilter),
                LastNameFilter = FormatFilter(request.LastNameFilter),
                EmailFilter = FormatFilter(request.EmailFilter),
                RestaurantIdFilter = FormatFilter(request.RestaurantIdFilter)
            });

        var totalRecords = await GetTotalRecords(connection, cancellationToken);

        var groupedResults = GroupResults(result, totalRecords);

        return groupedResults;
    }

    private static RegionalManagerDto GroupResults(
        IEnumerable<RegionalManagerIntermediateDto> result, int totalRecords)
    {
        var items = result.GroupBy(rm => new
        {
            rm.RegionalManagerId,
            rm.FirstName,
            rm.LastName,
            rm.Email,
            rm.NumberOfManagers
        })
        .Select(group => new RegionalManagerItem(
            group.Key.RegionalManagerId,
            group.Key.FirstName,
            group.Key.LastName,
            group.Key.Email,
            group.Key.NumberOfManagers,
            group.Select(rm => rm.RestaurantId).Distinct().ToList()));

        return new([.. items], totalRecords);
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
            ),
            FilteredManager AS (
                SELECT
                    *,
                    DENSE_RANK() OVER (PARTITION BY RegionalManagerId ORDER BY RegionalManagerId) AS RowNum
                FROM
                    ManagerCountCTE
                WHERE
                    FirstName LIKE @FirstNameFilter AND
                    LastName LIKE @LastNameFilter AND
                    Email LIKE @EmailFilter AND
                    RegionalManagerId LIKE @RegionalManagerIdFilter AND
                    RegionalManagerId IN (SELECT 
                        RegionalManagerId 
                        FROM ManagerCountCTE 
                        WHERE RestaurantId LIKE @RestaurantIdFilter)
            )
            SELECT
                RegionalManagerId,
                FirstName,
                LastName,
                Email,
                RestaurantId,
                NumberOfManagers
            FROM
                FilteredManager
            WHERE
                RowNum >= @Offset AND
                RowNum <= @Offset + @PageSize - 1";
    }

    private static async Task<int> GetTotalRecords(IDbConnection connection, CancellationToken cancellationToken)
    {
        var query = @"
            SELECT COUNT(1)
            FROM dbo.RegionalManagers WITH (NOLOCK)";

        var totalRecords = await connection.ExecuteScalarAsync<int>(query, cancellationToken);
        return totalRecords;
    }

    private static string FormatFilter(string? filter)
        => $"%{filter}%";

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
