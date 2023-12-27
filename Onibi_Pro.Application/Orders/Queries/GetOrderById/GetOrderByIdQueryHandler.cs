using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.MenuAggregate.Entities;
using Onibi_Pro.Domain.OrderAggregate;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;

namespace Onibi_Pro.Application.Orders.Queries.GetOrderById;
internal sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ErrorOr<IReadOnlyCollection<OrderPositionDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public GetOrderByIdQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<IReadOnlyCollection<OrderPositionDto>>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var order = await connection.QueryAsync<OrderPositionDto>(
            $"""
            SELECT mi.{nameof(MenuItem.Name)}, oi.{nameof(OrderItem.Quantity)},
                o.{nameof(Order.OrderTime)}, o.{nameof(Order.IsCancelled)}
            FROM dbo.Orders o
            JOIN dbo.OrderItem oi on o.Id = oi.OrderId
            JOIN dbo.MenuItems mi on mi.MenuItemId = oi.MenuItemId
            WHERE o.Id = @OrderId
            """, new { request.OrderId });

        if (order?.Any() != true)
        {
            return Errors.Order.OrderNotFound;
        }

        return order.ToArray();
    }
}
