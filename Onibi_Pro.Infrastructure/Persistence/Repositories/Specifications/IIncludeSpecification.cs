using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories.Specifications;
internal interface IIncludeSpecification<TAggregateRoot, TId> 
    where TAggregateRoot : AggregateRoot<TId>
    where TId : ValueObject
{
    IQueryable<TAggregateRoot> Include(IQueryable<TAggregateRoot> query);
}
