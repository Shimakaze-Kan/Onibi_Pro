namespace Onibi_Pro.Contracts.Statistics;
public record GetTopMenuItemsResponse(Guid RestaurantId, string MenuItemName, int OrdersCount);
