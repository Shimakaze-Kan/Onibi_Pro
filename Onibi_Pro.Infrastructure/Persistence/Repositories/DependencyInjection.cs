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
        services.AddScoped<IDomainRepository<Menu, MenuId>, DomainRepository<Menu, MenuId>>();
        services.AddScoped<IDomainRepository<Shipment, ShipmentId>, DomainRepository<Shipment, ShipmentId>>();
        services.AddScoped<IDomainRepository<Order, OrderId>, DomainRepository<Order, OrderId>>();
        services.AddScoped<IDomainRepository<Restaurant, RestaurantId>, DomainRepository<Restaurant, RestaurantId>>();
        services.AddScoped<IDomainRepository<User, UserId>, DomainRepository<User, UserId>>();

        services.AddScoped<IUserPasswordRepository, UserPasswordRepository>();

        return services;
    }
}
