using Onibi_Pro.Domain.MenuAggregate.Entities;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;
using Onibi_Pro.Domain.Models;

namespace Onibi_Pro.Domain.MenuAggregate;
public sealed class Menu : AggregateRoot<MenuId>
{
    private readonly List<MenuItem> _menuItems;

    public string Name { get; }
    public IReadOnlyList<MenuItem> MenuItems => _menuItems.ToList();

    private Menu(MenuId id, string name, List<MenuItem>? menuItems)
        : base(id)
    {
        Name = name;
        _menuItems = menuItems ?? new();
    }

    public static Menu Create(string name, List<MenuItem>? menuItems = null)
    {
        return new(MenuId.CreateUnique(), name, menuItems);
    }

    public void AddItem(MenuItem item)
    {
        _menuItems.Add(item);
    }

    public void RemoveItem(MenuItem item)
    {
        _menuItems.Remove(item);
    }
}
