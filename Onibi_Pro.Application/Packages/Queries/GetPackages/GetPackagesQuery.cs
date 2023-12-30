using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Packages.Queries.Common;

namespace Onibi_Pro.Application.Packages.Queries.GetPackages;
public record GetPackagesQuery(int StartRow, int Amount) : IRequest<GetPackagesDto>;