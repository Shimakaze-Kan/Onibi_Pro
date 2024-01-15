using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;

namespace Onibi_Pro.Application.Menus.Commands.AddMenuItem;
public record AddMenuItemCommand(MenuId MenuId, decimal Price, string Name, List<Ingredient> Ingredients) : IRequest<ErrorOr<Success>>;
