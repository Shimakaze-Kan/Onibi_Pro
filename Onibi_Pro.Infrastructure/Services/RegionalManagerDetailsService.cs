using Dapper;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Services;
internal sealed class RegionalManagerDetailsService : IRegionalManagerDetailsService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public RegionalManagerDetailsService(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<RegionalManagerDetailsDto> GetRegionalManagerDetailsAsync(UserId userId)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var query = @"
            SELECT rm.RegionalManagerId, r.Id AS RestaurantId, m.ManagerId
            FROM dbo.RegionalManagers rm
            JOIN dbo.RegionalManagerRestaurantIds rmri ON rmri.RegionalManagerId = rm.RegionalManagerId
            JOIN dbo.Restaurants r ON r.Id = rmri.RestaurantId
            LEFT JOIN dbo.Managers m ON m.RestaurantId = r.Id
            JOIN dbo.Users u ON u.Id = rm.UserId
            WHERE u.Id = @UserId";

        var results = await connection.QueryAsync(query, new { UserId = userId.Value });

        var groupedResults = results.GroupBy(
            r => r.RegionalManagerId,
            (key, g) => new RegionalManagerDetailsDto(
                key,
                g.Select(x => (Guid)x.RestaurantId).Distinct().ToList(),
                g.Select(x => (Guid?)x.ManagerId).OfType<Guid>().Distinct().ToList()
            )
        );

        return groupedResults.First();
    }

    public async Task<UserId> GetUserId(RegionalManagerId regionalManagerId)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var sql = @"
            SELECT u.Id
            FROM dbo.RegionalManagers m
            JOIN dbo.Users u on m.UserId = u.Id
            WHERE m.RegionalManagerId = @RegionalManagerId";

        var result = await connection.ExecuteScalarAsync<Guid>(sql, new { RegionalManagerId = regionalManagerId.Value });

        return UserId.Create(result);
    }
}
