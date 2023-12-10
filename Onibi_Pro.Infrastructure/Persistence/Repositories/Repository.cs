using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories;
internal sealed class Repository<TAggregateRoot, TId> : IRepository<TAggregateRoot, TId>
    where TAggregateRoot : AggregateRoot<TId>
    where TId : ValueObject
{
    private readonly OnibiProDbContext _dbContext;
    private readonly DbSet<TAggregateRoot> _dbSet;

    public Repository(OnibiProDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TAggregateRoot>();
    }

    public async Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return await _dbSet.SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken);
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
