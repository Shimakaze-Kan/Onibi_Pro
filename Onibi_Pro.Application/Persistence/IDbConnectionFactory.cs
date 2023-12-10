using System.Data;

namespace Onibi_Pro.Application.Persistence;
public interface IDbConnectionFactory
{
    Task<IDbConnection> OpenConnectionAsync();
}
