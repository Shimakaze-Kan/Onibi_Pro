using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories.Specifications;
internal interface ISpecificationProvider<TAggregateRoot, TId>
    where TAggregateRoot : AggregateRoot<TId>
    where TId : ValueObject
{
    IIncludeSpecification<TAggregateRoot, TId>? GetIncludeSpecification();
}