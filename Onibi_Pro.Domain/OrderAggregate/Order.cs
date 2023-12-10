using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;

namespace Onibi_Pro.Domain.OrderAggregate;
public sealed class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderItem> _orderItems = new();

    public DateTime OrderTime { get; private set; }
    public bool IsCancelled { get; private set; }
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.ToList();

    private Order(OrderId id, DateTime orderTime, bool isCancelled, List<OrderItem> orderItems)
        : base(id)
    {
        OrderTime = orderTime;
        IsCancelled = isCancelled;
        _orderItems = orderItems;
    }

    public static ErrorOr<Order> Create(DateTime orderTime, List<OrderItem> orderItems)
    {
        if (!orderItems.Any())
        {
            return Errors.Order.InvalidOrderItemAmount;
        }

        return new Order(OrderId.CreateUnique(), orderTime, false, orderItems);
    }

    public void AddItem(OrderItem item)
    {
        _orderItems.Add(item);
    }

    public void Cancel()
    {
        IsCancelled = true;
    }

    private Order() { }
}
