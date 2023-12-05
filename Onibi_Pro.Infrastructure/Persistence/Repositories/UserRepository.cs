using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Entities;

namespace Onibi_Pro.Infrastructure.Persistence.Repositories;
internal sealed class UserRepository : IUserRepository
{
    private static readonly List<User> _users = new();

    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetByEmail(string email)
    {
        return _users.SingleOrDefault(x => x.Email == email);
    }
}
