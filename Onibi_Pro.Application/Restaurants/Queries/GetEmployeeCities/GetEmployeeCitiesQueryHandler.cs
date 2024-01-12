using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Queries.GetEmployeeCities;
internal sealed class GetEmployeeCitiesQueryHandler : IRequestHandler<GetEmployeeCitiesQuery, ErrorOr<IReadOnlyCollection<string>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IManagerDetailsService _managerDetailsService;
    private readonly ICurrentUserService _currentUserService;

    public GetEmployeeCitiesQueryHandler(IDbConnectionFactory dbConnectionFactory,
        IManagerDetailsService managerDetailsService,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _managerDetailsService = managerDetailsService;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<IReadOnlyCollection<string>>> Handle(GetEmployeeCitiesQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var managerDetails = await _managerDetailsService.GetManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
        var restaurantId = managerDetails.RestaurantId;

        var sql = @"
            SELECT distinct City
            FROM dbo.Employees
            WHERE RestaurantId = @RestaurantId";

        var result = await connection.QueryAsync<string>(sql, new { restaurantId });

        return result.ToList();
    }
}
