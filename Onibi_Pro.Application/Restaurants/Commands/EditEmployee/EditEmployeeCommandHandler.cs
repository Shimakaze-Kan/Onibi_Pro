using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Commands.EditEmployee;
internal sealed class EditEmployeeCommandHandler : IRequestHandler<EditEmployeeCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public EditEmployeeCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<Success>> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.RestaurantRepository
            .GetByIdAsync(RestaurantId.Create(request.RestaurantId), cancellationToken);

        if (restaurant is null)
        {
            return Errors.Restaurant.RestaurantNotFound;
        }

        var positions = GetEmployeePositions(request);
        var employee = Employee.Create(EmployeeId.Create(request.EmployeeId), request.FirstName,
            request.LastName, request.Email, request.City, positions);

        var userId = UserId.Create(_currentUserService.UserId);
        var editRestult = restaurant.EditEmployee(userId, employee);

        if (editRestult.IsError)
        {
            return editRestult;
        }

        await _unitOfWork.RestaurantRepository.UpdateAsync(restaurant, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new Success();
    }

    private static List<EmployeePosition> GetEmployeePositions(EditEmployeeCommand request)
    {
        return request.EmployeePositions.ConvertAll(position => EmployeePosition.Create(Enum.Parse<Positions>(position)));
    }
}
