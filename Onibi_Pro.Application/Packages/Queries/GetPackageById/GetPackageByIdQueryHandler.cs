using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Packages.Queries.Common;
using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Application.Packages.Queries.GetPackageById;
internal sealed class GetPackageByIdQueryHandler : IRequestHandler<GetPackageByIdQuery, ErrorOr<PackageDto>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public GetPackageByIdQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<PackageDto>> Handle(GetPackageByIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var result = await connection.QueryFirstOrDefaultAsync<PackageDto>(
            "SELECT * FROM dbo.Packages WHERE PackageId = @packageId", new { PackageId = request.PackageId.Value });

        return result;
    }
}
