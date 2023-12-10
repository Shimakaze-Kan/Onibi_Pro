using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;
using Onibi_Pro.Domain.OrderAggregate;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;

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
        var menuItemIds = request.OrderItems.ConvertAll(item => item.MenuItemId);
        var areMenuItemIdsValid = await AreMenuItemIdsValid(menuItemIds);

        if (!areMenuItemIdsValid)
        {
            return Errors.Order.WrongMenuItemId;
        }

        var order = Order.Create(_dateTimeProvider.UtcNow,
            request.OrderItems.ConvertAll(item => OrderItem.Create(MenuItemId.Create(item.MenuItemId), item.Quantity)));

        if (order.IsError)
        {
            return order.Errors;
        }

        await _unitOfWork.OrderRepository.AddAsync(order.Value, cancellationToken);

        return order;
    }

    private async Task<bool> AreMenuItemIdsValid(List<Guid> menuItemIds)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        var query = "SELECT COUNT(*) FROM dbo.MenuItems WHERE MenuItemId IN @Ids";
        var count = await connection.QueryFirstOrDefaultAsync<int>(query, new { Ids = menuItemIds });

        return count == menuItemIds.Count;
    }
}
