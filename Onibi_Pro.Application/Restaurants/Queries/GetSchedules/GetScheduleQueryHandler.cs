using System.Data;

using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Application.Services.Access;
using Onibi_Pro.Domain.Common.Errors;

namespace Onibi_Pro.Application.Restaurants.Queries.GetSchedules;
internal sealed class GetScheduleQueryHandler : IRequestHandler<GetScheduleQuery, ErrorOr<IReadOnlyCollection<ScheduleDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IAccessService _accessService;
    private readonly ICurrentUserService _currentUserService;

    public GetScheduleQueryHandler(IDbConnectionFactory dbConnectionFactory,
        IAccessService accessService,
        ICurrentUserService currentUserService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _accessService = accessService;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<IReadOnlyCollection<ScheduleDto>>> Handle(GetScheduleQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var restaurantExists = await _accessService.RestauranExists(request.RestaurantId, connection);

        if (!restaurantExists)
        {
            return Errors.Restaurant.RestaurantNotFound;
        }

        var isRestaurantManager = await _accessService.IsUserRestaurantManager(
            request.RestaurantId, _currentUserService.UserId, connection);

        if (!isRestaurantManager)
        {
            return Errors.Restaurant.InvalidManager;
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
