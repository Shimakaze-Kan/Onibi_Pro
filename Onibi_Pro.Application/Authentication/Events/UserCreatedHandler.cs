using Dapper;

using MediatR;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate.Events;

namespace Onibi_Pro.Application.Authentication.Events;
internal sealed class UserCreatedHandler : INotificationHandler<UserCreated>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public UserCreatedHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        await connection.ExecuteAsync("INSERT INTO dbo.UserPasswords (UserId, Password) VALUES (@UserId, @Password)",
            new { UserId = notification.UserId.Value, Password = notification.HashedPassword });
    }
}
