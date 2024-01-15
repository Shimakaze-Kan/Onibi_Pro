using System.Data;

using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Application.Services.Access;
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
    private readonly IAccessService _accessService;
    private readonly ICurrentUserService _currentUserService;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IDbConnectionFactory dbConnectionFactory,
        IAccessService accessService,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _dbConnectionFactory = dbConnectionFactory;
        _accessService = accessService;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var restaurantExists = await _accessService.RestauranExists(request.RestaurantId, connection);

        if (!restaurantExists)
        {
            return Errors.Order.RestaurantNotFound;
        }

        var menuItemIds = request.OrderItems.ConvertAll(item => item.MenuItemId);
        var validMenuItemIds = await AreMenuItemIdsValid(menuItemIds, connection);

        var orderItems = new List<OrderItem>();

        foreach (var item in request.OrderItems)
        {
            var isMenuItemValid = validMenuItemIds.Contains(item.MenuItemId);
            var orderItem = OrderItem.Create(MenuItemId.Create(item.MenuItemId), item.Quantity, isMenuItemValid);

            if (orderItem.IsError)
            {
                return orderItem.Errors;
            }

            orderItems.Add(orderItem.Value);
        }
        
        var order = Order.Create(_dateTimeProvider.UtcNow,
            orderItems,
            RestaurantId.Create(request.RestaurantId));

        if (order.IsError)
        {
            return order.Errors;
        }

        await _unitOfWork.OrderRepository.AddAsync(order.Value, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return order;
    }

    private static async Task<IEnumerable<Guid>> AreMenuItemIdsValid(List<Guid> menuItemIds, IDbConnection connection)
    {
        var query = "SELECT MenuItemId FROM dbo.MenuItems WHERE MenuItemId IN @Ids AND IsDeleted = 0";
        return await connection.QueryAsync<Guid>(query, new { Ids = menuItemIds });
    }
}
