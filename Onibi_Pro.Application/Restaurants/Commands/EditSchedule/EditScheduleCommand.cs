using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Restaurants.Commands.EditSchedule;
public record EditScheduleCommand(Guid RestaurantId, Guid ScheduleId, string Title, string Priority,
    DateTime StartDate, DateTime EndDate, List<Guid> EmployeeIds) : IRequest<ErrorOr<Success>>;
