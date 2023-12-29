using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Commands.CreateRestaurant;
internal sealed class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, ErrorOr<Restaurant>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateRestaurantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Restaurant>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = Restaurant.Create(Address.Create(request.Address.Street, request.Address.City, request.Address.PostalCode, request.Address.Country),
            request.Employees?.ConvertAll(employee =>
                Employee.CreateUnique(employee.FirstName, employee.LastName, employee.Email, employee.City,
                    employee.EmployeePositions.ConvertAll(position =>
                    EmployeePosition.Create(Enum.Parse<Positions>(position.Position))))));

        await _unitOfWork.RestaurantRepository.AddAsync(restaurant, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return restaurant;
    }
}
