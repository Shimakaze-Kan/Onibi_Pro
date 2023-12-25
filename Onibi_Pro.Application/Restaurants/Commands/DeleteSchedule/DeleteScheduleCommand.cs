using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Restaurants.Commands.DeleteSchedule;
public record DeleteScheduleCommand(Guid RestaurantId, Guid ScheduleId) : IRequest<ErrorOr<Success>>;
