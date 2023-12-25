namespace Onibi_Pro.Contracts.Restaurants;

public record CreateScheduleRequest(string Title, string Priority,
    DateTime StartDate, DateTime EndDate, List<Guid> EmployeeIds);