using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Statistics.GetTopMenuItems;
public record GetTopMenuItemsQuery : IRequest<ErrorOr<IReadOnlyCollection<TopMenuItemsDto>>>;
