using System.Data;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Infrastructure.Persistence;
internal sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IDbConnection> OpenConnectionAsync(string clientName)
    {
        var connectionString = _configuration.GetConnectionString(
            $"SqlServerConnection_{clientName}");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        return connection;
    }
}
