using MediatR;

using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Queries.GetEmployeePositions;
internal sealed class GetEmployeePositionsQueryHandler : IRequestHandler<GetEmployeePositionsQuery, IReadOnlyCollection<string>>
{
    public async Task<IReadOnlyCollection<string>> Handle(GetEmployeePositionsQuery request, CancellationToken cancellationToken)
    {
        var names = Enum.GetNames<Positions>();

        return await Task.FromResult(names);
    }
}
