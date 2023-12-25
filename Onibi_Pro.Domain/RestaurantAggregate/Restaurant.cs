using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RestaurantAggregate;
public sealed class Restaurant : AggregateRoot<RestaurantId>
{
    private readonly List<OrderId> _orders = [];
    private readonly List<Employee> _employees = [];
    private readonly List<Manager> _managers = [];
    private readonly List<Schedule> _schedules = [];

    public Address Address { get; private set; }
    public IReadOnlyList<Employee> Employees => _employees.ToList();
    public IReadOnlyList<OrderId> OrderIds => _orders.ToList();
    public IReadOnlyList<Manager> Managers => _managers.ToList();
    public IReadOnlyList<Schedule> Schedules => _schedules.ToList();

    private Restaurant(RestaurantId id,
        Address address,
        List<Employee>? employees,
        List<OrderId>? orders,
        List<Manager>? managers)
        : base(id)
    {
        Address = address;
        _employees = employees ?? [];
        _orders = orders ?? [];
        _managers = managers ?? [];
    }

    public static Restaurant Create(Address address,
        List<Employee>? employees = null,
        List<OrderId>? orders = null,
        List<Manager>? managers = null)
    {
        return new(RestaurantId.CreateUnique(), address, employees, orders, managers);
    }

    public ErrorOr<Success> RegisterEmployee(UserId userId, Employee employee)
    {
        if (!_managers.Any(m => m.UserId == userId))
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

    public ErrorOr<Success> EditEmployee(UserId userId, Employee employee)
    {
        if (!_managers.Any(m => m.UserId == userId))
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

    public void UnregisterEmployee(UserId userId, Employee employee)
    {
        if (_managers.Any(m => m.UserId == userId))
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

    public ErrorOr<Success> AssigneManager(UserId userId, UserTypes userType)
    {
        if (userType != UserTypes.Manager)
        {
            return Errors.Restaurant.WrongUserManagerType;
        }

        if (_managers.Any(x => x.UserId == userId))
        {
            return Errors.Restaurant.UserIsAlreadyManager;
        }

        var manager = Manager.Create(userId);
        _managers.Add(manager);
        return new Success();
    }

    public void UnassigneManager(Manager manager)
    {
        _managers.Remove(manager);
    }

    public ErrorOr<Success> AddSchedule(UserId userId, Schedule schedule)
    {
        if (!_managers.Any(m => m.UserId == userId))
        {
            return Errors.Restaurant.InvalidManager;
        }

        if (schedule.Employees.Any(employeeId => !_employees.Any(employee => employee.Id == employeeId)))
        {
            return Errors.Restaurant.EmployeeNotFound;
        }

        _schedules.Add(schedule);

        return new Success();
    }

    public ErrorOr<Success> RemoveSchedule(UserId userId, ScheduleId scheduleId)
    {
        if (!_managers.Any(m => m.UserId == userId))
        {
            return Errors.Restaurant.InvalidManager;
        }

        var scheduleIndex = _schedules.FindIndex(schedule => schedule.Id == scheduleId);

        if (scheduleIndex == -1)
        {
            return Errors.Restaurant.ScheduleNotFound;
        }

        _schedules.RemoveAt(scheduleIndex);

        return new Success();
    }

    public ErrorOr<Success> UpdateSchedule(UserId userId, Schedule schedule)
    {
        if (!_managers.Any(m => m.UserId == userId))
        {
            return Errors.Restaurant.InvalidManager;
        }

        var scheduleIndex = _schedules.FindIndex(existingSchedule => existingSchedule.Id == schedule.Id);

        if (scheduleIndex == -1)
        {
            return Errors.Restaurant.ScheduleNotFound;
        }

        _schedules[scheduleIndex] = schedule;

        return new Success();
    }

#pragma warning disable CS8618
    private Restaurant() { }
#pragma warning restore CS8618
}
