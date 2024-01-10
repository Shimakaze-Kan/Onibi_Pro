namespace Onibi_Pro.Application.Persistence;
public interface IMasterDbRepository
{
    Task AddUser(string email, string clientName);
    Task AddClient(string name);
    Task<string> GetClientNameByUserEmail(string email);
    Task UpdateUser(string oldEmal, string newEmal, string clientName);
}
