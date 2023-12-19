using Microsoft.EntityFrameworkCore.Storage;

using Onibi_Pro.Application.Persistence;
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

namespace Onibi_Pro.Infrastructure.Persistence.Repositories;
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly OnibiProDbContext _dbContext;
    private IDbContextTransaction _currentTransaction;

    public IDomainRepository<Menu, MenuId> MenuRepository { get; }
    public IDomainRepository<Shipment, ShipmentId> ShipmentRepository { get; }
    public IDomainRepository<Order, OrderId> OrderRepository { get; }
    public IDomainRepository<Restaurant, RestaurantId> RestaurantRepository { get; }
    public IDomainRepository<User, UserId> UserRepository { get; }

    public UnitOfWork(OnibiProDbContext dbContext,
        IDomainRepository<Menu, MenuId> menuRepository,
        IDomainRepository<Shipment, ShipmentId> shipmentRepository,
        IDomainRepository<Order, OrderId> orderRepository,
        IDomainRepository<Restaurant, RestaurantId> restaurantRepository,
        IDomainRepository<User, UserId> userRepository)
    {
        _dbContext = dbContext;
        MenuRepository = menuRepository;
        ShipmentRepository = shipmentRepository;
        OrderRepository = orderRepository;
        RestaurantRepository = restaurantRepository;
        UserRepository = userRepository;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            return;
        }

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null!;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null!;
            }
        }
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
