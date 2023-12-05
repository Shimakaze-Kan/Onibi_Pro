using Onibi_Pro.Domain.Models;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RestaurantAggregate.Entities;
public sealed class Employee : Entity<EmployeeId>
{
    private readonly List<EmployeePosition> _positions = new();

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public IReadOnlyList<EmployeePosition> Positions => _positions.ToList();

    private Employee(EmployeeId id, string firstName, string lastName, string email)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Employee Create(string firstName, string lastName, string email)
    {
        return new(EmployeeId.CreateUnique(), firstName, lastName, email);
    }
}
