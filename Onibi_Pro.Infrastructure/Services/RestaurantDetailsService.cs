using Dapper;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Services;
internal sealed class RestaurantDetailsService : IRestaurantDetailsService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public RestaurantDetailsService(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<List<UserId>> GetManagerUserIds(RestaurantId restaurantId)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var sql = @"
            SELECT m.UserId
            FROM dbo.Managers m
            WHERE m.RestaurantId = @RestaurantId";

        var result = await connection.QueryAsync<Guid>(sql, new { RestaurantId = restaurantId.Value });

        return result.Select(UserId.Create).ToList();
    }
}
