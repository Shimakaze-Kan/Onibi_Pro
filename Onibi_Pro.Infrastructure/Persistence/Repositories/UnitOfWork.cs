using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.MenuAggregate;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;
using Onibi_Pro.Domain.OrderAggregate;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.ShipmentAggregate;
using Onibi_Pro.Domain.ShipmentAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories;
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly OnibiProDbContext _dbContext;

    public IRepository<Menu, MenuId> MenuRepository { get; }
    public IRepository<Shipment, ShipmentId> ShipmentRepository { get; }
    public IRepository<Order, OrderId> OrderRepository { get; }
    public IRepository<Restaurant, RestaurantId> RestaurantRepository { get; }

    public UnitOfWork(OnibiProDbContext dbContext)
    {
        _dbContext = dbContext;
        MenuRepository = new Repository<Menu, MenuId>(_dbContext);
        ShipmentRepository = new Repository<Shipment, ShipmentId>(_dbContext);
        OrderRepository = new Repository<Order, OrderId>(_dbContext);
        RestaurantRepository = new Repository<Restaurant, RestaurantId>(_dbContext);
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
