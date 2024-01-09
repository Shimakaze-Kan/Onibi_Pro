using Dapper;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Services;
internal sealed class CourierDetailsService : ICourierDetailsService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public CourierDetailsService(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<CourierDetailsDto> GetCourierDetailsAsync(UserId userId)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var sql = @"
            SELECT CourierId, RegionalManagerId, Phone
            FROM dbo.Couriers
            WHERE UserId = @UserId";

        var result = await connection.QueryFirstAsync<CourierDetailsDto>(sql, new { UserId = userId.Value });

        return result;
    }

    public async Task<UserId> GetUserId(CourierId courierId)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var sql = @"
            SELECT TOP 1 u.Id
            FROM dbo.Couriers c
            JOIN dbo.Users u on c.UserId = u.Id
            WHERE c.CourierId = @CourierId";

        var result = await connection.ExecuteScalarAsync<Guid>(sql, new { CourierId = courierId.Value });

        return UserId.Create(result);
    }
}
