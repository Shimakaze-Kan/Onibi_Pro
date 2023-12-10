using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Menus.Queries.GetMenus;
public record GetMenusQuery : IRequest<ErrorOr<IReadOnlyCollection<MenuDto>>>;
