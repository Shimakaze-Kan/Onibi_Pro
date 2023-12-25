using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Commands.DeleteSchedule;
internal sealed class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteScheduleCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<Success>> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(
            RestaurantId.Create(request.RestaurantId), cancellationToken);

        if (restaurant is null)
        {
            return Errors.Restaurant.RestaurantNotFound;
        }

        var removeScheduleResult = restaurant.RemoveSchedule(
            UserId.Create(_currentUserService.UserId), ScheduleId.Create(request.ScheduleId));

        if (removeScheduleResult.IsError)
        {
            return removeScheduleResult.Errors;
        }

        await _unitOfWork.SaveAsync(cancellationToken);
        return new Success();
    }
}
