using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Commands.EditEmployee;
internal sealed class EditEmployeeCommandHandler : IRequestHandler<EditEmployeeCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;

    public EditEmployeeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

        // check errors if (employee.)
        var managerId = ManagerId.Create(Guid.Parse("3A583B3E-3A5E-47DA-9009-13FE71345AB4")); // TODO get from current user service
        var editRestult = restaurant.EditEmployee(managerId, employee);

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
