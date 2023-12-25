using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Restaurants.Commands.CreateSchedule;
public record CreateScheduleCommand(Guid RestaurantId, string Title, string Priority, 
    DateTime StartDate, DateTime EndDate, List<Guid> EmployeeIds) : IRequest<ErrorOr<Success>>;