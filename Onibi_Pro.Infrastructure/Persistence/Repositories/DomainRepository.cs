using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories;
internal sealed class DomainRepository<TAggregateRoot, TId> : IDomainRepository<TAggregateRoot, TId>
    where TAggregateRoot : AggregateRoot<TId>
    where TId : ValueObject
{
    private readonly OnibiProDbContext _dbContext;
    private readonly DbSet<TAggregateRoot> _dbSet;

    public DomainRepository(DbContextFactory dbContextFactory,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContextFactory.CreateDbContext(currentUserService.ClientName);
        _dbSet = _dbContext.Set<TAggregateRoot>();
    }

    public async Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken, params string[] includes)
    {
        var query = _dbSet.AsQueryable();

        if (includes != null)
        {
            query = query.IncludeProperties(includes);
        }

        return await query.SingleOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);
    }

    public async Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(aggregateRoot, cancellationToken);
    }

    public async Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        _dbSet.Update(aggregateRoot);
        await Task.CompletedTask;
    }

    public async Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        _dbSet.Remove(aggregateRoot);
        await Task.CompletedTask;
    }

    public async Task<TAggregateRoot?> GetAsync(Expression<Func<TAggregateRoot, bool>> expression,
        CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(expression, cancellationToken);
    }
}
