using Dapper;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Services;
internal class ManagerDetailsService : IManagerDetailsService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public ManagerDetailsService(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ManagerDetailsDto> GetManagerDetailsAsync(UserId userId)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var query = @"
            SELECT m.ManagerId, m.RestaurantId, u.FirstName, u.LastName
            FROM dbo.Managers m
            JOIN dbo.Users u on m.UserId = u.Id
            WHERE m.RestaurantId = (
                SELECT TOP 1 im.RestaurantId FROM dbo.Managers im WHERE im.UserId = @UserId
            )"
        ;

        var managerDetails = await connection.QueryAsync(query, new { UserId = userId.Value });

        var managers = managerDetails.Select(m => new ManagerNames((string)m.FirstName, (string)m.LastName)).ToList();

        if (managers.Count == 0)
        {
            return new ManagerDetailsDto(Guid.Empty, Guid.Empty, new List<ManagerNames>());
        }

        var details = managerDetails.First();

        return new ManagerDetailsDto(details.ManagerId, details.RestaurantId, managers.AsReadOnly());
    }
}
