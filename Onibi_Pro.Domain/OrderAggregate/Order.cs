using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.OrderAggregate.Events;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.OrderAggregate;
public sealed class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderItem> _orderItems = [];

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

    public static ErrorOr<Order> Create(DateTime orderTime, List<OrderItem> orderItems, RestaurantId restaurantId)
    {
        if (!orderItems.Any())
        {
            return Errors.Order.InvalidOrderItemAmount;
        }

        var order = new Order(OrderId.CreateUnique(), orderTime, false, orderItems);
        order.AddDomainEvent(new OrderCreated(order.Id, restaurantId));

        return order;
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
