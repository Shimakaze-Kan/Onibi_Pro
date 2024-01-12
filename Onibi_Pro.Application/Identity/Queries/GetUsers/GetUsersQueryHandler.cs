using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Application.Identity.Queries.GetUsers;
internal class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ErrorOr<IReadOnlyCollection<UserDataDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;

    public GetUsersQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<IReadOnlyCollection<UserDataDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var dynamicParameters = new DynamicParameters();
        dynamicParameters.Add("@Query", request.Query);

        var query = @"
                SELECT TOP 20 [Id]
                      ,[FirstName]
                      ,[LastName]
                      ,[Email]
                      ,[UserType]
                  FROM [dbo].[Users]
                  WHERE [Email] LIKE '%' + @Query + '%'";

        if (Guid.TryParse(request.UserId, out var userId))
        {
            query += " AND Id = @UserId";
            dynamicParameters.Add("@UserId", userId);
        }

        var users = await connection.QueryAsync<UserDataDto>(query, dynamicParameters);

        return users.ToList();
    }
}
