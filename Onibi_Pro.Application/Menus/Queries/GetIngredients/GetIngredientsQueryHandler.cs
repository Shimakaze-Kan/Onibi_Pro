using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Application.Menus.Queries.GetIngredients;
internal sealed class GetIngredientsQueryHandler : IRequestHandler<GetIngredientsQuery, ErrorOr<IReadOnlyCollection<IngredientKeyValueDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public GetIngredientsQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<IReadOnlyCollection<IngredientKeyValueDto>>> Handle(GetIngredientsQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var result = await connection.QueryAsync<IngredientKeyValueDto>("SELECT distinct Name, Unit FROM dbo.Ingredients");

        return result.ToList();
    }
}
