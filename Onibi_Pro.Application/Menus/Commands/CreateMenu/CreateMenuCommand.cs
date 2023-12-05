using ErrorOr;
using MediatR;
using Onibi_Pro.Domain.MenuAggregate;

namespace Onibi_Pro.Application.Menus.Commands.CreateMenu;
public record CreateMenuCommand(string Name, List<MenuItemCommand> MenuItems) : IRequest<ErrorOr<Menu>>;

public record MenuItemCommand(string Name, decimal Price, List<IngredientCommand> Ingredients);

public record IngredientCommand(string Name, string Unit, decimal Quantity);
