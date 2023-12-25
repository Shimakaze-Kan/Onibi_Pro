using System.Data;

using Dapper;

namespace Onibi_Pro.Application.Services.Access;
internal sealed class AccessService : IAccessService
{
    public async Task<bool> IsUserRestaurantManager(Guid restaurantId, Guid userId, IDbConnection connection)
    {
        return await connection.ExecuteScalarAsync<bool>(
            @"SELECT ISNULL((SELECT 1 FROM dbo.Managers
                WHERE RestaurantId = @restaurantId
                AND UserId = @userId), 0)", new { restaurantId, userId });
    }

    public async Task<bool> RestauranExists(Guid restaurantId, IDbConnection connection)
    {
        return await connection.ExecuteScalarAsync<bool>(
            "SELECT ISNULL((SELECT 1 FROM dbo.Restaurants WHERE Id = @RestaurantId), 0)", new { restaurantId });
    }
}
