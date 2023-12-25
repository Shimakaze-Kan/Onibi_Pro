namespace Onibi_Pro.Contracts.Restaurants;

public record EditScheduleRequest(Guid ScheduleId, string Title, string Priority,
    DateTime StartDate, DateTime EndDate, List<Guid> EmployeeIds);