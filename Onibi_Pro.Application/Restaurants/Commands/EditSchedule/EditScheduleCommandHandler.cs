using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Commands.EditSchedule;
internal sealed class EditScheduleCommandHandler : IRequestHandler<EditScheduleCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public EditScheduleCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<Success>> Handle(EditScheduleCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(
            RestaurantId.Create(request.RestaurantId), cancellationToken);

        if (restaurant is null)
        {
            return Errors.Restaurant.RestaurantNotFound;
        }

        var priority = Enum.Parse<Priorities>(request.Priority, ignoreCase: true);
        var employeeIds = request.EmployeeIds.ConvertAll(EmployeeId.Create);
        var schedule = Schedule.Create(ScheduleId.Create(request.ScheduleId), request.Title,
            request.StartDate, request.EndDate, priority, employeeIds);

        if (schedule.IsError)
        {
            return schedule.Errors;
        }

        var updateScheduleResult = restaurant.UpdateSchedule(UserId.Create(_currentUserService.UserId), schedule.Value);

        if (updateScheduleResult.IsError)
        {
            return updateScheduleResult.Errors;
        }

        await _unitOfWork.SaveAsync(cancellationToken);
        return new Success();
    }
}
