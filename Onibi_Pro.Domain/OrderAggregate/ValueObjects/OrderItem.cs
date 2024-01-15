using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;


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

    public static ErrorOr<OrderItem> Create(MenuItemId menuItemId, int quantity, bool menuItemExistsAndIsNotDeleted)
    {
        if (!menuItemExistsAndIsNotDeleted)
        {
            return Errors.Order.WrongMenuItemId;
        }

        return new OrderItem(menuItemId, quantity);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MenuItemId;
        yield return Quantity;
    }

#pragma warning disable CS8618
    private OrderItem() { }
#pragma warning restore CS8618
}
