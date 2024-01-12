using System.Data;

using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Packages.Queries.Common;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Queries.GetPackages;
internal sealed class GetPackagesQueryHandler : IRequestHandler<GetPackagesQuery, ErrorOr<GetPackagesDto>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;
    private readonly IManagerDetailsService _managerDetailsService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;
    private readonly ICourierDetailsService _courierDetailsService;

    public GetPackagesQueryHandler(IDbConnectionFactory dbConnectionFactory,
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

    public async Task<ErrorOr<GetPackagesDto>> Handle(GetPackagesQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        DynamicParameters dynamicParameters = new();
        dynamicParameters.Add("@PageNumber", request.StartRow / request.Amount + 1);
        dynamicParameters.Add("@PageSize", request.Amount);

        (var whereClause, dynamicParameters) = _currentUserService.UserType switch
        {
            UserTypes.Manager => await SetupManagerWhereClause(dynamicParameters),
            UserTypes.RegionalManager => await SetupRegionalManagerWhereClause(dynamicParameters),
            UserTypes.Courier => await SetupCourierWhereClause(dynamicParameters),
            _ => (string.Empty, dynamicParameters)
        };

        var packages = await GetPackages(connection, whereClause, dynamicParameters);
        var total = await GetTotalCount(connection, whereClause, dynamicParameters);

        return new GetPackagesDto([.. packages], total);
    }

    private static async Task<IEnumerable<PackageDto>> GetPackages(IDbConnection connection, string whereClause, DynamicParameters dynamicParameters)
    {
        var query = @$"
            SELECT p.[PackageId]
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
            FROM 
              [Onibi_Pro].[dbo].[Packages] p
            LEFT JOIN dbo.Couriers c on c.CourierId = p.Courier
            {whereClause}
            ORDER BY 
              [IsUrgent] DESC,
              [Until] DESC
            OFFSET (@PageNumber - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS ONLY;";

        var result = await connection.QueryAsync<PackageDto>(query, dynamicParameters);
        return result;
    }

    private async Task<(string, DynamicParameters)> SetupRegionalManagerWhereClause(DynamicParameters dynamicParameters)
    {
        var regionalManagerDetails =
            await _regionalManagerDetailsService.GetRegionalManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
        var whereClause = @"
                WHERE
                [RegionalManager] = @RegionalManagerId";

        dynamicParameters.Add("@RegionalManagerId", regionalManagerDetails.RegionalManagerId);
        return (whereClause, dynamicParameters);
    }

    private async Task<(string, DynamicParameters)> SetupManagerWhereClause(DynamicParameters dynamicParameters)
    {
        var managerDetails = await _managerDetailsService.GetManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
        var whereClause = @"
                WHERE
                [SourceRestaurant] = @RestaurantId
                OR [DestinationRestaurant] = @RestaurantId";

        dynamicParameters.Add("@RestaurantId", managerDetails.RestaurantId);
        return (whereClause, dynamicParameters);
    }

    private async Task<(string, DynamicParameters)> SetupCourierWhereClause(DynamicParameters dynamicParameters)
    {
        var courierDetails = await _courierDetailsService.GetCourierDetailsAsync(UserId.Create(_currentUserService.UserId));
        var whereClause = @"
            WHERE Courier = @CourierId";

        dynamicParameters.Add("@CourierId", courierDetails.CourierId);
        return (whereClause, dynamicParameters);
    }

    private static async Task<long> GetTotalCount(IDbConnection connection, string whereClause, DynamicParameters dynamicParameters)
    {
        var query = @$"
            SELECT COUNT(1)
            FROM dbo.Packages WITH (NOLOCK)
            {whereClause}";

        return await connection.ExecuteScalarAsync<long>(query, dynamicParameters);
    }
}
