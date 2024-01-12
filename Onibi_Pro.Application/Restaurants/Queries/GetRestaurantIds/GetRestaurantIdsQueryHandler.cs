using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Application.Restaurants.Queries.GetRestaurantIds;
internal sealed class GetRestaurantIdsQueryHandler : IRequestHandler<GetRestaurantIdsQuery, ErrorOr<IReadOnlyCollection<Guid>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public GetRestaurantIdsQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<IReadOnlyCollection<Guid>>> Handle(GetRestaurantIdsQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var result = await connection.QueryAsync<Guid>("SELECT Id FROM dbo.Restaurants", cancellationToken);

        return result.ToList();
    }
}
