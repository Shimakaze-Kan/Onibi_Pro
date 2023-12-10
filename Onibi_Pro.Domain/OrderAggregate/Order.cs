using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;

namespace Onibi_Pro.Domain.OrderAggregate;
public sealed class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderItem> _orderItems = new();

    public DateTime OrderTime { get; private set; }
    public bool IsCancelled { get; private set; }
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.ToList();

    private Order(OrderId id, DateTime orderTime, bool isCancelled)
        : base(id)
    {
        OrderTime = orderTime;
        IsCancelled = isCancelled;
    }

    public static Order Create()
    {
        return new(OrderId.CreateUnique(), DateTime.UtcNow, false);
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
