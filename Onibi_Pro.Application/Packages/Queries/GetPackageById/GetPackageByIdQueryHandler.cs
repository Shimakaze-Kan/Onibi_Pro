using System.Data;

using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Packages.Queries.Common;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Queries.GetPackageById;
internal sealed class GetPackageByIdQueryHandler : IRequestHandler<GetPackageByIdQuery, ErrorOr<PackageDto>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;
    private readonly IManagerDetailsService _managerDetailsService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;

    public GetPackageByIdQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService,
        IManagerDetailsService managerDetailsService,
        IRegionalManagerDetailsService regionalManagerDetailsService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
        _managerDetailsService = managerDetailsService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
    }

    public async Task<ErrorOr<PackageDto>> Handle(GetPackageByIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        DynamicParameters dynamicParameters = new DynamicParameters();
        dynamicParameters.Add("@PackageId", request.PackageId.Value);

        (var andClause, dynamicParameters) = _currentUserService.UserType switch
        {
            UserTypes.Manager => await SetupManagerAndClause(dynamicParameters),
            UserTypes.RegionalManager => await SetupRegionalManagerAndClause(dynamicParameters),
            _ => (string.Empty, dynamicParameters)
        };

        var result = await GetPackage(connection, dynamicParameters, andClause);

        if (result is null)
        {
            return Errors.Package.PackageNotFound;
        }

        return result;
    }

    private static async Task<PackageDto?> GetPackage(IDbConnection connection, DynamicParameters dynamicParameters, string andClause)
    {
        return await connection.QueryFirstOrDefaultAsync<PackageDto>(
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
            FROM dbo.Packages WHERE PackageId = @packageId
            {andClause}", dynamicParameters);
    }

    private async Task<(string, DynamicParameters)> SetupRegionalManagerAndClause(DynamicParameters dynamicParameters)
    {
        var regionalManagerDetails =
            await _regionalManagerDetailsService.GetRegionalManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
        var whereClause = @"
                AND
                [RegionalManager] = @RegionalManagerId";

        dynamicParameters.Add("@RegionalManagerId", regionalManagerDetails.RegionalManagerId);
        return (whereClause, dynamicParameters);
    }

    private async Task<(string, DynamicParameters)> SetupManagerAndClause(DynamicParameters dynamicParameters)
    {
        var managerDetails = await _managerDetailsService.GetManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
        var whereClause = @"
                AND
                [SourceRestaurant] = @RestaurantId
                OR [DestinationRestaurant] = @RestaurantId";

        dynamicParameters.Add("@RestaurantId", managerDetails.RestaurantId);
        return (whereClause, dynamicParameters);
    }
}
