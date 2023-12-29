using static Onibi_Pro.Application.Orders.Queries.GetOrders.OrdersDto;

namespace Onibi_Pro.Application.Orders.Queries.GetOrders;

public record OrdersDto(IReadOnlyCollection<OrderDto> Orders, long TotalCount)
{
    public record OrderDto(Guid OrderId, DateTime OrderTime, DateTime? CancelledTime, bool IsCancelled)
    {
        public IReadOnlyList<OrderItemDto> OrderItems { get; init; } = [];
        public decimal Total { get; init; } = 0;
    }

    public record OrderItemDto(Guid MenuItemId, int Quantity, string MenuItemName, decimal Price)
    {
        public decimal Sum => Quantity * Price;
    }
}
