using MediatR;

namespace Onibi_Pro.Application.Restaurants.Queries.GetEmployeePositions;
public record GetEmployeePositionsQuery : IRequest<IReadOnlyCollection<string>>;