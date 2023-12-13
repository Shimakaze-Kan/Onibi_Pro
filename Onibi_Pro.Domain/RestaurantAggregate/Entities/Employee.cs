using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RestaurantAggregate.Entities;
public sealed class Employee : Entity<EmployeeId>
{
    private readonly List<EmployeePosition> _positions = new();

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string City { get; private set; }
    public IReadOnlyList<EmployeePosition> Positions => _positions.ToList();

    private Employee(EmployeeId id, string firstName, string lastName, string email,
        string city, List<EmployeePosition> employeePositions)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        City = city;
        _positions = employeePositions;
    }

    public static Employee CreateUnique(string firstName, string lastName, string email,
        string city, List<EmployeePosition> employeePositions)
    {
        return new(EmployeeId.CreateUnique(), firstName, lastName, email, city, employeePositions);
    }

    public static Employee Create(EmployeeId id, string firstName, string lastName, string email,
        string city, List<EmployeePosition> employeePositions)
    {
        return new(id, firstName, lastName, email, city, employeePositions);
    }

#pragma warning disable CS8618
    private Employee() { }
#pragma warning restore CS8618
}
