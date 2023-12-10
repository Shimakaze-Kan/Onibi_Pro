using System.Data;

using Microsoft.Data.SqlClient;

using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Infrastructure.Persistence;
internal sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> OpenConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        return connection;
    }
}
