﻿using Mapster;

using Onibi_Pro.Application.Orders.Commands.CreateOrder;
using Onibi_Pro.Contracts.Orders;
using Onibi_Pro.Domain.OrderAggregate;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;

namespace Onibi_Pro.Mapping;

public class OrderMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateOrderRequest request, Guid restaurantId), CreateOrderCommand>()
            .Map(dest => dest, src => src.request)
            .Map(dest => dest.RestaurantId, src => src.restaurantId);

        config.NewConfig<Order, CreateOrderResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
        
        config.NewConfig<OrderItem, OrderItemResponse>()
            .Map(dest => dest.MenuItemId, src => src.MenuItemId.Value);
    }
}
