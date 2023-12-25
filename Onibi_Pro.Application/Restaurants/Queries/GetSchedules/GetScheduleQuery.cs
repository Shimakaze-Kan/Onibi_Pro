using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Restaurants.Queries.GetSchedules;
public record GetScheduleQuery(Guid RestaurantId) : IRequest<ErrorOr<IReadOnlyCollection<ScheduleDto>>>;