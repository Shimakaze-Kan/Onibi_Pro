using Microsoft.Extensions.DependencyInjection;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.MenuAggregate;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;
using Onibi_Pro.Domain.OrderAggregate;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.PackageAggregate;
using Onibi_Pro.Domain.PackageAggregate.ValueObjects;
using Onibi_Pro.Domain.RegionalManagerAggregate;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;
using Onibi_Pro.Infrastructure.Persistence.Repositories.Specifications;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories;
internal static class DependencyInjection
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSpecifications();

        services.AddScoped<IDomainRepository<Menu, MenuId>, DomainRepository<Menu, MenuId>>();
        services.AddScoped<IDomainRepository<Order, OrderId>, DomainRepository<Order, OrderId>>();
        services.AddScoped<IDomainRepository<Restaurant, RestaurantId>, DomainRepository<Restaurant, RestaurantId>>();
        services.AddScoped<IDomainRepository<User, UserId>, DomainRepository<User, UserId>>();
        services.AddScoped<IDomainRepository<RegionalManager, RegionalManagerId>, DomainRepository<RegionalManager, RegionalManagerId>>();
        services.AddScoped<IDomainRepository<Package, PackageId>, DomainRepository<Package, PackageId>>();

        services.AddScoped<IUserPasswordRepository, UserPasswordRepository>();

        return services;
    }

    private static IServiceCollection AddSpecifications(this IServiceCollection services)
    {
        services.AddScoped<ISpecificationProvider<RegionalManager, RegionalManagerId>, RegionalManagerSpecificationProvider>();

        return services;
    }
}
