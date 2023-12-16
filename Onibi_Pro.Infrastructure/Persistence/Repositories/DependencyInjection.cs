using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.ShipmentAggregate.ValueObjects;
using Onibi_Pro.Domain.ShipmentAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;
using Onibi_Pro.Domain.MenuAggregate;
using Onibi_Pro.Domain.OrderAggregate;
using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.UserAggregate;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories;
internal static class DependencyInjection
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Menu, MenuId>, Repository<Menu, MenuId>>();
        services.AddScoped<IRepository<Shipment, ShipmentId>, Repository<Shipment, ShipmentId>>();
        services.AddScoped<IRepository<Order, OrderId>, Repository<Order, OrderId>>();
        services.AddScoped<IRepository<Restaurant, RestaurantId>, Repository<Restaurant, RestaurantId>>();
        services.AddScoped<IRepository<User, UserId>, Repository<User, UserId>>();

        return services;
    }
}
