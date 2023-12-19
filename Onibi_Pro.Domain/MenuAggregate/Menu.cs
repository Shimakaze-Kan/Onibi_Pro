using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.MenuAggregate.Entities;
using Onibi_Pro.Domain.MenuAggregate.Events;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;

namespace Onibi_Pro.Domain.MenuAggregate;
public sealed class Menu : AggregateRoot<MenuId>
{
    private readonly List<MenuItem> _menuItems;

    public string Name { get; private set; }
    public IReadOnlyList<MenuItem> MenuItems => _menuItems.ToList();

    private Menu(MenuId id, string name, List<MenuItem> menuItems)
        : base(id)
    {
        Name = name;
        _menuItems = menuItems ?? [];
    }

    public static ErrorOr<Menu> Create(string name, List<MenuItem> menuItems)
    {
        if (name.Length is 0 or > 100)
        {
            return Errors.Menu.InvalidName;
        }

        if (menuItems?.Any() != true)
        {
            return Errors.Menu.InvalidAmountOfMenuItems;
        }

        Menu menu = new(MenuId.CreateUnique(), name, menuItems);
        menu.AddDomainEvent(new MenuCreated(menu));

        return menu;
    }

    public void AddItem(MenuItem item)
    {
        _menuItems.Add(item);
    }

    public void RemoveItem(MenuItem item)
    {
        _menuItems.Remove(item);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Menu() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
