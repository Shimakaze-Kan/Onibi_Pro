using Onibi_Pro.Domain.MenuAggregate.ValueObjects;
using Onibi_Pro.Domain.Models;

namespace Onibi_Pro.Domain.OrderAggregate.ValueObjects;
public sealed class OrderItem : ValueObject
{
    public MenuItemId MenuItemId { get; }
    public int Quantity { get; }

    private OrderItem(MenuItemId menuItemId, int quantity)
    {
        MenuItemId = menuItemId;
        Quantity = quantity;
    }

    public static OrderItem Create(MenuItemId menuItemId, int quantity)
    {
        return new OrderItem(menuItemId, quantity);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MenuItemId;
        yield return Quantity;
    }
}
