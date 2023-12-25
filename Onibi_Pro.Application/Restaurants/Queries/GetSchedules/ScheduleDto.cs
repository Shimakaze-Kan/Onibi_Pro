namespace Onibi_Pro.Application.Restaurants.Queries.GetSchedules;
public record ScheduleDto(Guid ScheduleId, string Title, string Priority,
    DateTime StartDate, DateTime EndDate)
{
    public IReadOnlyCollection<Guid> EmployeeIds { get; init; } = new List<Guid>();
}
