using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.MenuAggregate;

namespace Onibi_Pro.Infrastructure.Persistence;
internal sealed class MenuRepository : IMenuRepository
{
    private static readonly List<Menu> Menus = new();

    public void Add(Menu menu)
    {
        Menus.Add(menu);
    }
}
