using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.MenuAggregate.Entities;
using Onibi_Pro.Domain.OrderAggregate;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;

namespace Onibi_Pro.Application.Orders.Queries.GetOrderById;
internal sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ErrorOr<IReadOnlyCollection<OrderDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetOrderByIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ErrorOr<IReadOnlyCollection<OrderDto>>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        var order = await connection.QueryAsync<OrderDto>(
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
