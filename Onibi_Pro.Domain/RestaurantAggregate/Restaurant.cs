using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RestaurantAggregate;
public sealed class Restaurant : AggregateRoot<RestaurantId>
{
    private readonly List<OrderId> _orders = new();
    private readonly List<Employee> _employees = new();

    public Address Address { get; }
    public IReadOnlyList<Employee> Employees => _employees.ToList();
    public IReadOnlyList<OrderId> OrderIds => _orders.ToList();

    private Restaurant(RestaurantId id, Address address)
        : base(id)
    {
        Address = address;
    }

    public static Restaurant Create(Address address)
    {
        return new(RestaurantId.CreateUnique(), address);
    }

    public void AddEmployee(Employee employee)
    {
        // add logic
        _employees.Add(employee);
    }

    public void RemoveEmployee(Employee employee)
    {
        // logic
        _employees.Remove(employee);
    }

#pragma warning disable CS8618
    private Restaurant() { }
#pragma warning restore CS8618
}
