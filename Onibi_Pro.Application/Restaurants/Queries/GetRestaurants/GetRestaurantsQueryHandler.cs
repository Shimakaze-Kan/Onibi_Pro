using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Application.Restaurants.Queries.GetRestaurants;
internal sealed class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, ErrorOr<IReadOnlyCollection<RestaurantDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public GetRestaurantsQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<IReadOnlyCollection<RestaurantDto>>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var sql = @$"
            SELECT [Id] AS {nameof(RestaurantDto.Id)}
                  ,[Address_Street] AS {nameof(RestaurantDto.Street)}
                  ,[Address_City] AS {nameof(RestaurantDto.City)}
                  ,[Address_PostalCode] AS {nameof(RestaurantDto.PostalCode)}
                  ,[Address_Country] AS {nameof(RestaurantDto.Country)}
            FROM dbo.[Restaurants]
            WHERE Id LIKE @Query
            OR Address_Street LIKE @Query
            OR Address_City LIKE @Query
            OR Address_PostalCode LIKE @Query
            OR Address_Country LIKE @Query";

        var result = await connection.QueryAsync<RestaurantDto>(sql, new { Query = $"%{request.Query}%" });

        return result.ToList();
    }
}
