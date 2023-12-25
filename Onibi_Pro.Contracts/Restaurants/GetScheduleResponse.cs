namespace Onibi_Pro.Contracts.Restaurants;

public record GetScheduleResponse(Guid ScheduleId, string Title, string Priority,
    DateTime StartDate, DateTime EndDate, IReadOnlyCollection<Guid> EmployeeIds);
