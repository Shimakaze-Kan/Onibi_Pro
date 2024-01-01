using System.Data;

using Dapper;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Packages.Queries.Common;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Queries.GetPackages;
internal sealed class GetPackagesQueryHandler : IRequestHandler<GetPackagesQuery, GetPackagesDto>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;
    private readonly IManagerDetailsService _managerDetailsService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;

    public GetPackagesQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService,
        IManagerDetailsService managerDetailsService,
        IRegionalManagerDetailsService regionalManagerDetailsService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
        _managerDetailsService = managerDetailsService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
    }

    public async Task<GetPackagesDto> Handle(GetPackagesQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        DynamicParameters dynamicParameters = new();
        dynamicParameters.Add("@PageNumber", request.StartRow / request.Amount + 1);
        dynamicParameters.Add("@PageSize", request.Amount);

        (var whereClause, dynamicParameters) = _currentUserService.UserType switch
        {
            UserTypes.Manager => await SetupManagerWhereClause(dynamicParameters),
            UserTypes.RegionalManager => await SetupRegionalManagerWhereClause(dynamicParameters),
            _ => (string.Empty, dynamicParameters)
        };

        var packages = await GetPackages(connection, whereClause, dynamicParameters);
        var total = await GetTotalCount(connection, whereClause, dynamicParameters);

        return new([.. packages], total);
    }

    private static async Task<IEnumerable<PackageDto>> GetPackages(IDbConnection connection, string whereClause, DynamicParameters dynamicParameters)
    {
        var query = @$"
            SELECT [PackageId]
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
              ,[AvailableTransitions] AS {nameof(PackageDto.AvailableTransitions)}
            FROM 
              [Onibi_Pro].[dbo].[Packages]
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

    private static async Task<long> GetTotalCount(IDbConnection connection, string whereClause, DynamicParameters dynamicParameters)
    {
        var query = @$"
            SELECT COUNT(1)
            FROM dbo.Packages WITH (NOLOCK)
            {whereClause}";

        return await connection.ExecuteScalarAsync<long>(query, dynamicParameters);
    }
}
