using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Infrastructure.Persistence.Repositories.Specifications;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories;
internal class DomainRepository<TAggregateRoot, TId> : IDomainRepository<TAggregateRoot, TId>
    where TAggregateRoot : AggregateRoot<TId>
    where TId : ValueObject
{
    private readonly OnibiProDbContext _dbContext;
    private readonly DbSet<TAggregateRoot> _dbSet;
    private readonly ISpecificationProvider<TAggregateRoot, TId>? _specificationProvider;

    public DomainRepository(DbContextFactory dbContextFactory,
        ICurrentUserService currentUserService,
        ISpecificationProvider<TAggregateRoot, TId>? specificationProvider = null)
    {
        _dbContext = dbContextFactory.CreateDbContext(currentUserService.ClientName);
        _dbSet = _dbContext.Set<TAggregateRoot>();
        _specificationProvider = specificationProvider;
    }

    public async Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        IQueryable<TAggregateRoot> query = _dbSet;

        var includeSpecification = _specificationProvider?.GetIncludeSpecification();
        if (includeSpecification != null)
        {
            query = includeSpecification.Include(query);
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
