using Onibi_Pro.Domain.MenuAggregate;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;
using Onibi_Pro.Domain.OrderAggregate;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.ShipmentAggregate;
using Onibi_Pro.Domain.ShipmentAggregate.ValueObjects;

namespace Onibi_Pro.Application.Persistence;
public interface IUnitOfWork
{
    IRepository<Menu, MenuId> MenuRepository { get; }
    IRepository<Shipment, ShipmentId> ShipmentRepository { get; }
    IRepository<Order, OrderId> OrderRepository { get; }
    // todo rest of repos
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}
