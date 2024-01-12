using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Packages.Queries.GetPackages;
public record GetPackagesQuery(int StartRow, int Amount) : IRequest<ErrorOr<GetPackagesDto>>;