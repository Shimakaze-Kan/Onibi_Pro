using System.Data;
using System.Text.Json;

using Dapper;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

using static Onibi_Pro.Application.Orders.Queries.GetOrders.OrdersDto;

namespace Onibi_Pro.Application.Orders.Queries.GetOrders;
internal sealed class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, OrdersDto>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;
    private readonly IManagerDetailsService _managerDetailsService;

    public GetOrdersQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService,
        IManagerDetailsService managerDetailsService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
        _managerDetailsService = managerDetailsService;
    }

    public async Task<OrdersDto> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var managerDetails = await _managerDetailsService.GetManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
        var restaurantId = managerDetails.RestaurantId;
        var endRow = request.StartRow + request.Amount;

        var orders = await GetOrders(request, connection, restaurantId, endRow);
        var totalCount = await GetTotalCount(connection, restaurantId);

        return new(orders, totalCount);
    }

    private static async Task<IReadOnlyCollection<OrderDto>> GetOrders(
        GetOrdersQuery request, IDbConnection connection, Guid restaurantId, int endRow)
    {
        var query = @"
        WITH OrderedOrders AS (
            SELECT 
                o.Id AS OrderId, 
                o.OrderTime, 
                o.IsCancelled,
                ROW_NUMBER() OVER (ORDER BY o.OrderTime DESC) AS RowNum
            FROM dbo.Orders o
            JOIN dbo.OrderIds oid ON oid.OrderId = o.Id
            WHERE oid.RestaurantId = @restaurantId
        )
        SELECT 
            oo.OrderId,
            oo.OrderTime,
            oo.IsCancelled,
            (
                SELECT 
                    oi.MenuItemId, 
                    oi.Quantity, 
                    mi.Name AS MenuItemName, 
                    mi.Price
                FROM dbo.OrderItem oi
                JOIN dbo.MenuItems mi ON mi.MenuItemId = oi.MenuItemId
                WHERE oi.OrderId = oo.OrderId
                FOR JSON PATH
            ) AS OrderItems
        FROM OrderedOrders oo
        WHERE oo.RowNum >= @startRow AND oo.RowNum < @endRow
        ORDER BY oo.RowNum;";

        var orderDictionary = new Dictionary<Guid, OrderDto>();

        await connection.QueryAsync<OrderDto, string, OrderDto>(
            query,
            (order, json) =>
            {
                if (!orderDictionary.TryGetValue(order.OrderId, out var orderEntry))
                {
                    orderEntry = order with { OrderItems = new List<OrderItemDto>() };
                    orderDictionary.Add(orderEntry.OrderId, orderEntry);
                }

                if (!string.IsNullOrEmpty(json))
                {
                    var orderItems = JsonSerializer.Deserialize<List<OrderItemDto>>(json);
                    var total = orderItems?.Sum(item => item.Price * item.Quantity) ?? 0;
                    orderEntry = orderEntry with { OrderItems = orderItems ?? [], Total = total };
                    orderDictionary[orderEntry.OrderId] = orderEntry;
                }

                return orderEntry;
            },
            new { restaurantId, request.StartRow, endRow },
            splitOn: "OrderItems"
        );

        return [.. orderDictionary.Values];
    }

    private static async Task<long> GetTotalCount(IDbConnection connection, Guid restaurantId)
    {
        var query = @"
            SELECT COUNT(1)
            FROM dbo.OrderIds WITH (NOLOCK)
            WHERE RestaurantId = @restaurantId;";

        return await connection.ExecuteScalarAsync<long>(query, new { restaurantId });
    }
}
