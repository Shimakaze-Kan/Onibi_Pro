using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Commands.CreateRestaurant;
internal sealed class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, ErrorOr<Restaurant>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CreateRestaurantCommandHandler(IUnitOfWork unitOfWork,
        IDbConnectionFactory dbConnectionFactory)
    {
        _unitOfWork = unitOfWork;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ErrorOr<Restaurant>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var orderIds = request.OrderIds;

        if (orderIds != null)
        {
            var areOrderIdsValid = await AreOrderIdsValid(orderIds);
            if (!areOrderIdsValid)
            {
                return Errors.Restaurant.InvalidOrderId;
            }
        }

        var restaurant = Restaurant.Create(Address.Create(request.Address.Street, request.Address.City, request.Address.PostalCode, request.Address.Country),
            request.Employees?.ConvertAll(employee =>
                Employee.CreateUnique(employee.FirstName, employee.LastName, employee.Email, employee.City,
                    employee.EmployeePositions.ConvertAll(position =>
                    EmployeePosition.Create(Enum.Parse<Positions>(position.Position))))),
            request.OrderIds?.ConvertAll(OrderId.Create),
            request.Managers?.ConvertAll(manager => Manager.Create(UserId.Create(Guid.NewGuid()))));

        await _unitOfWork.RestaurantRepository.AddAsync(restaurant, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return restaurant;
    }

    private async Task<bool> AreOrderIdsValid(List<Guid> orderIds)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        var query = "SELECT COUNT(*) FROM dbo.Orders WHERE Id IN @Ids";
        var count = await connection.QueryFirstOrDefaultAsync<int>(query, new { Ids = orderIds });

        return count == orderIds.Count;
    }
}
