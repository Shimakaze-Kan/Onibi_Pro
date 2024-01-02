using Onibi_Pro.Domain.RegionalManagerAggregate;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories.Specifications;
internal class RegionalManagerSpecificationProvider : ISpecificationProvider<RegionalManager, RegionalManagerId>
{
    public IIncludeSpecification<RegionalManager, RegionalManagerId>? GetIncludeSpecification()
    {
        return new RegionalManagerWithCouriersSpecification();
    }
}
