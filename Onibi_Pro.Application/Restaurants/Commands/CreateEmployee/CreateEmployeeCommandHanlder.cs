using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Commands.CreateEmployee;
internal sealed class CreateEmployeeCommandHanlder : IRequestHandler<CreateEmployeeCommand, ErrorOr<Employee>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateEmployeeCommandHanlder(IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<Employee>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.RestaurantRepository
            .GetByIdAsync(RestaurantId.Create(request.RestaurantId), cancellationToken);

        if (restaurant is null)
        {
            return Errors.Restaurant.RestaurantNotFound;
        }

        var userId = UserId.Create(_currentUserService.UserId);
        var positions = GetEmployeePositions(request);

        var employee = Employee.CreateUnique(request.FirstName, request.LastName,
            request.Email, request.City, positions);

        var result = restaurant.RegisterEmployee(userId, employee);

        if (result.IsError)
        {
            return result.Errors;
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return employee;
    }

    private static List<EmployeePosition> GetEmployeePositions(CreateEmployeeCommand request)
    {
        return request.EmployeePositions.ConvertAll(position => EmployeePosition.Create(Enum.Parse<Positions>(position)));
    }
}
