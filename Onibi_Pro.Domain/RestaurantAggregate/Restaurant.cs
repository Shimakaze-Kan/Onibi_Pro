using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RestaurantAggregate;
public sealed class Restaurant : AggregateRoot<RestaurantId>
{
    private readonly List<OrderId> _orders = new();
    private readonly List<Employee> _employees = new();
    private readonly List<Manager> _managers = new();

    public Address Address { get; private set; }
    public IReadOnlyList<Employee> Employees => _employees.ToList();
    public IReadOnlyList<OrderId> OrderIds => _orders.ToList();
    public IReadOnlyList<Manager> Managers => _managers.ToList();

    private Restaurant(RestaurantId id,
        Address address,
        List<Employee>? employees,
        List<OrderId>? orders,
        List<Manager>? managers)
        : base(id)
    {
        Address = address;
        _employees = employees ?? new();
        _orders = orders ?? new();
        _managers = managers ?? new();
    }

    public static Restaurant Create(Address address,
        List<Employee>? employees = null,
        List<OrderId>? orders = null,
        List<Manager>? managers = null)
    {
        return new(RestaurantId.CreateUnique(), address, employees, orders, managers);
    }

    public ErrorOr<Success> RegisterEmployee(ManagerId manager, Employee employee)
    {
        if (!_managers.Any(m => m.Id == manager))
        {
            return Errors.Restaurant.InvalidManager;
        }

        if (_employees.Any(e => e.Email == employee.Email))
        {
            return Errors.Restaurant.DuplicatedEmail;
        }
        _employees.Add(employee);

        return new Success();
    }

    public ErrorOr<Success> EditEmployee(ManagerId manager, Employee employee)
    {
        if (!_managers.Any(m => m.Id == manager))
        {
            return Errors.Restaurant.InvalidManager;
        }

        var employeeIndex = _employees.FindIndex(e => e.Id == employee.Id);

        if (employeeIndex == -1)
        {
            return Errors.Restaurant.EmployeeNotFound;
        }

        if (_employees.Any(e => e.Email == employee.Email && e.Id != employee.Id))
        {
            return Errors.Restaurant.DuplicatedEmail;
        }

        _employees[employeeIndex] = employee;

        return new Success();
    }

    public void UnregisterEmployee(Manager manager, Employee employee)
    {
        if (_managers.Contains(manager))
        {
            _employees.Remove(employee);
        }
    }

    public void AddOrders(List<OrderId> orders)
    {
        _orders.AddRange(orders);
    }

    public void RemoveOrder(OrderId orderId)
    {
        _orders.Remove(orderId);
    }

#pragma warning disable CS8618
    private Restaurant() { }
#pragma warning restore CS8618
}
