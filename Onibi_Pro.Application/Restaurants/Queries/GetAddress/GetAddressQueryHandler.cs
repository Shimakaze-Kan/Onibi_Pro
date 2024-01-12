using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Queries.GetAddress;
internal sealed class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, ErrorOr<Address>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public GetAddressQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<Address>> Handle(GetAddressQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var result = await connection.QueryFirstAsync<Address>(@$"
            SELECT Address_Street AS {nameof(Address.Street)},
	            Address_City AS {nameof(Address.City)},
	            Address_PostalCode AS {nameof(Address.PostalCode)},
	            Address_Country AS {nameof(Address.Country)}
            FROM dbo.Restaurants
            WHERE Id = @RestaurantId", new { RestaurantId = request.RestaurantId.Value });

        return result;
    }
}
