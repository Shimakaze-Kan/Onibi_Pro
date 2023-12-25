using System.Data;

using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Onibi_Pro.Application.Persistence;

namespace Onibi_Pro.Infrastructure.MasterDb.Repositories;
internal sealed class MasterDbRepository : IMasterDbRepository
{
    private const string MasterDbConnectionStringName = "SqlServerConnection_Onibi_MasterDb";
    private readonly string _connectionString;

    public MasterDbRepository(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(MasterDbConnectionStringName);

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        _connectionString = connectionString;
    }

    public async Task AddClient(string name)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@Name", name);

        connection.Execute("dbo.AddClient", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task AddUser(string email, string clientName)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@Email", email);
        parameters.Add("@ClientName", clientName);

        connection.Execute("dbo.AddUser", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<string> GetClientNameByUserEmail(string email)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@Email", email);
        parameters.Add("@ClientName", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

        connection.Execute("dbo.GetClientNameByEmail", parameters, commandType: CommandType.StoredProcedure);

        return parameters.Get<string>("@ClientName");
    }
}
