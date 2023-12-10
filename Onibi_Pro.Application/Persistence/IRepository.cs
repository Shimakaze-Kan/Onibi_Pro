using System.Linq.Expressions;

using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Application.Persistence;
public interface IRepository<TAggregateRoot, TId>
    where TAggregateRoot : AggregateRoot<TId>
    where TId : ValueObject
{
    Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);
    Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);
    Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);
    Task<TAggregateRoot?> GetAsync(Expression<Func<TAggregateRoot, bool>> expression,
        CancellationToken cancellationToken = default);
}
