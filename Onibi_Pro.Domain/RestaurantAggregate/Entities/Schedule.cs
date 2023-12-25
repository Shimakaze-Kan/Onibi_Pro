using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RestaurantAggregate.Entities;
public sealed class Schedule : Entity<ScheduleId>
{
    private readonly List<EmployeeId> _employeeIds = [];

    public Priorities Priority { get; private set; }
    public string Title { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public IReadOnlyList<EmployeeId> Employees => _employeeIds.ToList();

    private Schedule(ScheduleId id, string title, DateTime startDate, DateTime endDate,
        Priorities priotitiy, List<EmployeeId> employeeIds) : base(id)
    {
        Title = title;
        StartDate = startDate;
        EndDate = endDate;
        Priority = priotitiy;
        _employeeIds = employeeIds;
    }

    public static ErrorOr<Schedule> Create(ScheduleId id, string title, DateTime startDate, DateTime endDate,
        Priorities priotitiy, List<EmployeeId> employeeIds)
    {
        if (endDate.Date < startDate.Date)
        {
            return Errors.Restaurant.EndDateBeforeStartDate;
        }

        if (title.Length is < 3 or > 125)
        {
            return Errors.Restaurant.WrongScheduleTitleLength;
        }

        if (employeeIds.Count == 0)
        {
            return Errors.Restaurant.ScheduleEmployeeNumber;
        }

        return new Schedule(id, title, startDate, endDate, priotitiy, employeeIds);
    }

    public static ErrorOr<Schedule> CreateUnique(string title, DateTime startDate, DateTime endDate,
        Priorities priotitiy, List<EmployeeId> employeeIds)
    {
        return Create(ScheduleId.CreateUnique(), title, startDate, endDate, priotitiy, employeeIds);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Schedule() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

public enum Priorities
{
    Standard,
    Essential,
    Critical
}