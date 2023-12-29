using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.OrderAggregate;
public sealed class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderItem> _orderItems = [];

    public DateTime OrderTime { get; private set; }
    public DateTime? CancelledTime { get; private set; }
    public bool IsCancelled { get; private set; }
    public RestaurantId RestaurantId { get; private set; }
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.ToList();

    private Order(OrderId id, DateTime orderTime, bool isCancelled, RestaurantId restaurantId, List<OrderItem> orderItems)
        : base(id)
    {
        OrderTime = orderTime;
        IsCancelled = isCancelled;
        _orderItems = orderItems;
        CancelledTime = null;
        RestaurantId = restaurantId;
    }

    public static ErrorOr<Order> Create(DateTime orderTime, List<OrderItem> orderItems, RestaurantId restaurantId)
    {
        if (!orderItems.Any())
        {
            return Errors.Order.InvalidOrderItemAmount;
        }

        return new Order(OrderId.CreateUnique(), orderTime, false, restaurantId, orderItems);
    }

    public void AddItem(OrderItem item)
    {
        _orderItems.Add(item);
    }

    public ErrorOr<Success> Cancel(DateTime currentTime)
    {
        if (IsCancelled)
        {
            return Errors.Order.AlreadyCancelled;
        }

        IsCancelled = true;
        CancelledTime = currentTime;

        return new Success();
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Order() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
