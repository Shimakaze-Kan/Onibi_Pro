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
    private readonly ICourierDetailsService _courierDetailsService;

    public GetPackageByIdQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService,
        IManagerDetailsService managerDetailsService,
        IRegionalManagerDetailsService regionalManagerDetailsService,
        ICourierDetailsService courierDetailsService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
        _managerDetailsService = managerDetailsService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
        _courierDetailsService = courierDetailsService;
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
            UserTypes.Courier => await SetupCourierAndClause(dynamicParameters),
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
            @$"SELECT p.[PackageId]
              ,p.[DestinationRestaurant] AS {nameof(PackageDto.DestinationRestaurant)}
              ,p.[Manager] AS {nameof(PackageDto.Manager)}
              ,p.[RegionalManager] AS {nameof(PackageDto.RegionalManager)}
              ,p.[SourceRestaurant] AS {nameof(PackageDto.SourceRestaurant)}
              ,p.[Courier] AS {nameof(PackageDto.Courier)}
              ,p.[Origin_Street] AS {nameof(PackageDto.OriginStreet)}
              ,p.[Origin_City] AS {nameof(PackageDto.OriginCity)}
              ,p.[Origin_PostalCode] AS {nameof(PackageDto.OriginPostalCode)}
              ,p.[Origin_Country] AS {nameof(PackageDto.OriginCountry)}
              ,p.[Destination_Street] AS {nameof(PackageDto.DestinationStreet)}
              ,p.[Destination_City] AS {nameof(PackageDto.DestinationCity)}
              ,p.[Destination_PostalCode] AS {nameof(PackageDto.DestinationPostalCode)}
              ,p.[Destination_Country] AS {nameof(PackageDto.DestinationCountry)}
              ,p.[Status] AS {nameof(PackageDto.Status)}
              ,p.[Message] AS {nameof(PackageDto.Message)}
              ,p.[IsUrgent] AS {nameof(PackageDto.IsUrgent)}
              ,c.[Phone] AS {nameof(PackageDto.CourierPhone)}
              ,p.[Ingredients] AS {nameof(PackageDto.Ingredients)}
              ,p.[Until] AS {nameof(PackageDto.Until)}
              ,p.[AvailableTransitions] AS {nameof(PackageDto.AvailableTransitions)}
            FROM dbo.Packages p
            LEFT JOIN dbo.Couriers c on c.CourierId = p.Courier
            WHERE PackageId = @packageId
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

    private async Task<(string, DynamicParameters)> SetupCourierAndClause(DynamicParameters dynamicParameters)
    {
        var courierDetails = await _courierDetailsService.GetCourierDetailsAsync(UserId.Create(_currentUserService.UserId));
        var whereClause = @"
            AND Courier = @CourierId";

        dynamicParameters.Add("@CourierId", courierDetails.CourierId);
        return (whereClause, dynamicParameters);
    }
}
