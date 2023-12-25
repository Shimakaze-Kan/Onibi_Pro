using System.Data;

using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Persistence;

using Onibi_Pro.Domain.Common.Errors;

namespace Onibi_Pro.Application.Restaurants.Queries.GetSchedules;
internal sealed class GetScheduleQueryHandler : IRequestHandler<GetScheduleQuery, ErrorOr<IReadOnlyCollection<ScheduleDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetScheduleQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ErrorOr<IReadOnlyCollection<ScheduleDto>>> Handle(GetScheduleQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(); // TODO check if user is manager of restaurant

        var restaurantExists = await connection.ExecuteScalarAsync<bool>(
            "SELECT ISNULL((SELECT 1 FROM dbo.Restaurants WHERE Id = @RestaurantId), 0)", new { request.RestaurantId });

        if (!restaurantExists)
        {
            return Errors.Restaurant.RestaurantNotFound;
        }

        List<ScheduleDto> schedules = await GetSchedules(connection, request.RestaurantId);

        return schedules;
    }

    private static async Task<List<ScheduleDto>> GetSchedules(IDbConnection connection, Guid restaurantId)
    {
        string query = @$"
          SELECT s.ScheduleId AS {nameof(ScheduleDto.ScheduleId)},
            s.Title AS {nameof(ScheduleDto.Title)},
            s.Priority AS {nameof(ScheduleDto.Priority)},
            s.StartDate AS {nameof(ScheduleDto.StartDate)},
            s.EndDate AS {nameof(ScheduleDto.EndDate)},
            es.EmployeeId
          FROM dbo.Schedules s
          JOIN dbo.EmployeesSchedules es on s.ScheduleId = es.ScheduleId
          WHERE s.RestaurantId = @RestaurantId
          ORDER BY s.StartDate DESC";

        var scheduleDictionary = new Dictionary<Guid, ScheduleDto>();

        await connection.QueryAsync<ScheduleDto, Guid, ScheduleDto>(
            query,
            (schedule, employeeId) =>
            {
                if (!scheduleDictionary.TryGetValue(schedule.ScheduleId, out var scheduleEntry))
                {
                    scheduleEntry = schedule;
                    scheduleEntry = scheduleEntry with { EmployeeIds = new List<Guid>() };
                    scheduleDictionary.Add(scheduleEntry.ScheduleId, scheduleEntry);
                }

                var employeeIds = scheduleEntry.EmployeeIds.ToList();
                employeeIds.Add(employeeId);

                scheduleEntry = scheduleEntry with { EmployeeIds = employeeIds };
                scheduleDictionary[schedule.ScheduleId] = scheduleEntry;
                return scheduleEntry;
            },
            new { restaurantId },
            splitOn: "EmployeeId"
        );

        return [.. scheduleDictionary.Values];
    }
}
