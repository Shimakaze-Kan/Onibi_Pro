using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.Events;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RestaurantAggregate;
public sealed class Restaurant : AggregateRoot<RestaurantId>
{
    private readonly List<Employee> _employees = [];
    private readonly List<Manager> _managers = [];
    private readonly List<Schedule> _schedules = [];

    public Address Address { get; private set; }
    public IReadOnlyList<Employee> Employees => _employees.ToList();
    public IReadOnlyList<Manager> Managers => _managers.ToList();
    public IReadOnlyList<Schedule> Schedules => _schedules.ToList();

    private Restaurant(RestaurantId id,
        Address address,
        List<Employee>? employees,
        List<Manager>? managers)
        : base(id)
    {
        Address = address;
        _employees = employees ?? [];
        _managers = managers ?? [];
    }

    public static Restaurant Create(Address address,
        RegionalManagerId regionalManagerId,
        List<Employee>? employees = null,
        List<Manager>? managers = null)
    {
        var restaurant = new Restaurant(RestaurantId.CreateUnique(), address, employees, managers);

        restaurant.AddDomainEvent(new RestaurantCreated(restaurant, regionalManagerId));
        return restaurant;
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

    internal ErrorOr<Success> AssignManager(UserId userId)
    {
        if (_managers.Any(x => x.UserId == userId))
        {
            return Errors.Restaurant.UserIsAlreadyManager;
        }

        var manager = Manager.Create(userId);
        _managers.Add(manager);
        return new Success();
    }

    public ErrorOr<Success> UnassignManager(Manager manager)
    {
        if (!_managers.Contains(manager))
        {
            return Errors.Restaurant.ManagerNotFound;
        }

        if (_managers.Count == 1)
        {
            return Errors.Restaurant.NoManagersLeft;
        }

        _managers.Remove(manager);
        return new Success();
    }

    public Manager? TryGetManager(UserId userId)
    {
        return _managers.FirstOrDefault(manager => manager.UserId == userId);
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
