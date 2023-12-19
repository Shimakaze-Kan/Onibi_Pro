using System.Data;

using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;
using Onibi_Pro.Domain.OrderAggregate;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.Orders.Commands.CreateOrder;
internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ErrorOr<Order>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IDbConnectionFactory dbConnectionFactory)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ErrorOr<Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        var restaurantExists = await connection.ExecuteScalarAsync<bool>(
            "SELECT ISNULL((SELECT 1 FROM dbo.Restaurants WHERE Id = @RestaurantId), 0)", new { request.RestaurantId });

        if (!restaurantExists)
        {
            return Errors.Order.RestaurantNotFound;
        }

        var menuItemIds = request.OrderItems.ConvertAll(item => item.MenuItemId);
        var areMenuItemIdsValid = await AreMenuItemIdsValid(menuItemIds, connection);

        if (!areMenuItemIdsValid)
        {
            return Errors.Order.WrongMenuItemId;
        }

        var order = Order.Create(_dateTimeProvider.UtcNow,
            request.OrderItems.ConvertAll(item => OrderItem.Create(MenuItemId.Create(item.MenuItemId), item.Quantity)),
            RestaurantId.Create(request.RestaurantId));

        if (order.IsError)
        {
            return order.Errors;
        }

        await _unitOfWork.OrderRepository.AddAsync(order.Value, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return order;
    }

    private static async Task<bool> AreMenuItemIdsValid(List<Guid> menuItemIds, IDbConnection connection)
    {
        var query = "SELECT COUNT(*) FROM dbo.MenuItems WHERE MenuItemId IN @Ids";
        var count = await connection.QueryFirstOrDefaultAsync<int>(query, new { Ids = menuItemIds });

        return count == menuItemIds.Count;
    }
}
