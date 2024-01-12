using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.RegionalManagers.Queries.GetRegionalManagers;
public record GetRegionalManagersQuery(int PageNumber, int PageSize) : IRequest<ErrorOr<IReadOnlyCollection<RegionalManagerDto>>>;