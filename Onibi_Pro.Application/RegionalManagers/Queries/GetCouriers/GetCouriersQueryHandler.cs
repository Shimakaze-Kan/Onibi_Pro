using Dapper;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Application.RegionalManagers.Queries.Common;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.RegionalManagers.Queries.GetCouriers;
internal sealed class GetCouriersQueryHandler : IRequestHandler<GetCouriersQuery, IReadOnlyCollection<CourierDto>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;

    public GetCouriersQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService,
        IRegionalManagerDetailsService regionalManagerDetailsService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
    }

    public async Task<IReadOnlyCollection<CourierDto>> Handle(GetCouriersQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var regionalManagerDetails = await _regionalManagerDetailsService.GetRegionalManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
        var sql = @"
            SELECT c.CourierId, c.Phone, u.FirstName, u.LastName, u.Email
            FROM dbo.Couriers c
            JOIN dbo.Users u on u.Id = c.UserId
            WHERE c.RegionalManagerId = @RegionalManagerId";

        var result = await connection.QueryAsync<CourierDto>(sql, new { regionalManagerDetails.RegionalManagerId });

        return result.ToList();
    }
}
