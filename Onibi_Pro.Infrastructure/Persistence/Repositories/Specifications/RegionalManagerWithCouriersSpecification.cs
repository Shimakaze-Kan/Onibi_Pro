using Microsoft.EntityFrameworkCore;

using Onibi_Pro.Domain.RegionalManagerAggregate;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories.Specifications;
internal class RegionalManagerWithCouriersSpecification 
    : IIncludeSpecification<RegionalManager, RegionalManagerId>
{
    public IQueryable<RegionalManager> Include(IQueryable<RegionalManager> query)
    {
        return query.Include(rm => rm.Couriers);
    }
}
