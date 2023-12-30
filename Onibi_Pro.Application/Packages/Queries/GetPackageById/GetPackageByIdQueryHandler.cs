using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Packages.Queries.Common;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;

namespace Onibi_Pro.Application.Packages.Queries.GetPackageById;
internal sealed class GetPackageByIdQueryHandler : IRequestHandler<GetPackageByIdQuery, ErrorOr<PackageDto>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;
    private readonly IManagerDetailsService _managerDetailsService;

    public GetPackageByIdQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService,
        IManagerDetailsService managerDetailsService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
        _managerDetailsService = managerDetailsService;
    }

    public async Task<ErrorOr<PackageDto>> Handle(GetPackageByIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var result = await connection.QueryFirstOrDefaultAsync<PackageDto>(
            @$"SELECT [PackageId]
              ,[DestinationRestaurant] AS {nameof(PackageDto.DestinationRestaurant)}
              ,[Manager] AS {nameof(PackageDto.Manager)}
              ,[RegionalManager] AS {nameof(PackageDto.RegionalManager)}
              ,[SourceRestaurant] AS {nameof(PackageDto.SourceRestaurant)}
              ,[Courier] AS {nameof(PackageDto.Courier)}
              ,[Origin_Street] AS {nameof(PackageDto.OriginStreet)}
              ,[Origin_City] AS {nameof(PackageDto.OriginCity)}
              ,[Origin_PostalCode] AS {nameof(PackageDto.OriginPostalCode)}
              ,[Origin_Country] AS {nameof(PackageDto.OriginCountry)}
              ,[Destination_Street] AS {nameof(PackageDto.DestinationStreet)}
              ,[Destination_City] AS {nameof(PackageDto.DestinationCity)}
              ,[Destination_PostalCode] AS {nameof(PackageDto.DestinationPostalCode)}
              ,[Destination_Country] AS {nameof(PackageDto.DestinationCountry)}
              ,[Status] AS {nameof(PackageDto.Status)}
              ,[Message] AS {nameof(PackageDto.Message)}
              ,[IsUrgent] AS {nameof(PackageDto.IsUrgent)}
              ,[Ingredients] AS {nameof(PackageDto.Ingredients)}
              ,[Until] AS {nameof(PackageDto.Until)}
            FROM dbo.Packages WHERE PackageId = @packageId", new { PackageId = request.PackageId.Value });

        if (result is null)
        {
            return Errors.Package.PackageNotFound;
        }

        return result;
    }
}
