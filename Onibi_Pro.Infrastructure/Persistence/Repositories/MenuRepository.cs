using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.MenuAggregate;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories;
internal sealed class MenuRepository : IMenuRepository
{
    private readonly OnibiProDbContext _onibiProDbContext;

    public MenuRepository(OnibiProDbContext onibiProDbContext)
    {
        _onibiProDbContext = onibiProDbContext;
    }

    public void Add(Menu menu)
    {
        _onibiProDbContext.Add(menu);

        _onibiProDbContext.SaveChanges();
    }
}
