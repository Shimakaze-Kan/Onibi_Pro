using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Commands.CreateEmployee;
internal sealed class CreateEmployeeCommandHanlder : IRequestHandler<CreateEmployeeCommand, ErrorOr<Employee>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmployeeCommandHanlder(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Employee>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.RestaurantRepository
            .GetByIdAsync(RestaurantId.Create(request.RestaurantId), cancellationToken);

        if (restaurant is null)
        {
            return Errors.Restaurant.RestaurantNotFound;
        }

        var managerId = ManagerId.Create(Guid.Parse("3A583B3E-3A5E-47DA-9009-13FE71345AB4")); // TODO get from current user service
        var positions = GetEmployeePositions(request);

        var employee = Employee.Create(request.FirstName, request.LastName,
            request.Email, request.City, positions);

        var result = restaurant.RegisterEmployee(managerId, employee);

        if (result.IsError)
        {
            return result.Errors;
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        return employee;
    }

    private static List<EmployeePosition> GetEmployeePositions(CreateEmployeeCommand request)
    {
        return request.EmployeePositions.ConvertAll(position => EmployeePosition.Create(Enum.Parse<Positions>(position)));
    }
}
