using Onibi_Pro.Domain.MenuAggregate;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;
using Onibi_Pro.Domain.OrderAggregate;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.ShipmentAggregate;
using Onibi_Pro.Domain.ShipmentAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Persistence;
public interface IUnitOfWork
{
    IDomainRepository<Menu, MenuId> MenuRepository { get; }
    IDomainRepository<Order, OrderId> OrderRepository { get; }
    IDomainRepository<Restaurant, RestaurantId> RestaurantRepository { get; }
    IDomainRepository<Shipment, ShipmentId> ShipmentRepository { get; }
    IDomainRepository<User, UserId> UserRepository { get; }

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
