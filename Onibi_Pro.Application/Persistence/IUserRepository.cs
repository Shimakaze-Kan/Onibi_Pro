using Onibi_Pro.Domain.Entities;

namespace Onibi_Pro.Application.Persistence;
public interface IUserRepository
{
    User? GetByEmail(string email);
    void Add(User user);
}
