using ErrorOr;

using MediatR;

using Onibi_Pro.Application.RegionalManagers.Queries.Common;

namespace Onibi_Pro.Application.RegionalManagers.Queries.GetCouriers;
public record GetCouriersQuery : IRequest<ErrorOr<IReadOnlyCollection<CourierDto>>>;