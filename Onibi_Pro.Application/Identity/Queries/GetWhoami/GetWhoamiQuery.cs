using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Identity.Queries.GetWhoami;
public record GetWhoamiQuery : IRequest<ErrorOr<WhoamiDto>>;