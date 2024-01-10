using Dapper;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.RegionalManagers.Queries.GetManagers;
internal sealed class GetManagersQueryHandler : IRequestHandler<GetManagersQuery, IReadOnlyCollection<ManagerDto>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;
    private readonly ICurrentUserService _currentUserService;

    public GetManagersQueryHandler(IDbConnectionFactory dbConnectionFactory,
        IRegionalManagerDetailsService regionalManagerDetailsService,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _regionalManagerDetailsService = regionalManagerDetailsService;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyCollection<ManagerDto>> Handle(GetManagersQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);
        var regionalManagerDetails = await _regionalManagerDetailsService.GetRegionalManagerDetailsAsync(UserId.Create(_currentUserService.UserId));

        var sql = @$"
            SELECT m.ManagerId AS {nameof(ManagerDto.ManagerId)},
	               m.RestaurantId AS {nameof(ManagerDto.RestaurantId)},
	               u.FirstName AS {nameof(ManagerDto.FirstName)},
	               u.LastName AS {nameof(ManagerDto.LastName)},
	               u.Email AS {nameof(ManagerDto.Email)}
            FROM dbo.RegionalManagerRestaurantIds rmi
            JOIN dbo.Managers m on m.RestaurantId = rmi.RestaurantId
            JOIN dbo.Users u on m.UserId = u.Id
            WHERE rmi.RegionalManagerId = @RegionalManagerId
                AND m.ManagerId like @ManagerIdFilter
                AND m.RestaurantId like @RestaurantIdFilter
                AND u.FirstName like @FirstNameFilter
                AND u.LastName like @LastNameFilter
                AND u.Email like @EmailFilter";

        var result = await connection.QueryAsync<ManagerDto>(sql,
            new
            {
                regionalManagerDetails.RegionalManagerId,
                ManagerIdFilter = FormatFilter(request.ManagerIdFilter),
                RestaurantIdFilter = FormatFilter(request.RestaurantIdFilter),
                FirstNameFilter = FormatFilter(request.FirstNameFilter),
                LastNameFilter = FormatFilter(request.LastNameFilter),
                EmailFilter = FormatFilter(request.EmailFilter)
            });

        return result.ToList();
    }

    private static string FormatFilter(string? filter)
        => $"%{filter}%";
}
