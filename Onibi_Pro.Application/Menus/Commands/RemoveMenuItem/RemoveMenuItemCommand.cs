using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.MenuAggregate.ValueObjects;

namespace Onibi_Pro.Application.Menus.Commands.RemoveMenuItem;
public record RemoveMenuItemCommand(MenuId MenuId, MenuItemId MenuItemId) : IRequest<ErrorOr<Success>>;
