using System.Data;

using Dapper;

using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Queries.GetEmployees;
internal sealed class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, ErrorOr<IReadOnlyCollection<EmployeeDto>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICurrentUserService _currentUserService;
    private readonly IManagerDetailsService _managerDetailsService;

    public GetEmployeesQueryHandler(IDbConnectionFactory dbConnectionFactory,
        ICurrentUserService currentUserService,
        IManagerDetailsService managerDetailsService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _currentUserService = currentUserService;
        _managerDetailsService = managerDetailsService;
    }

    public async Task<ErrorOr<IReadOnlyCollection<EmployeeDto>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);

        var managerDetails = await _managerDetailsService.GetManagerDetailsAsync(UserId.Create(_currentUserService.UserId));

        if (managerDetails.RestaurantId != request.RestaurantId)
        {
            return Errors.Restaurant.RestaurantNotFound;
        }

        var result = await GetEmployees(request, connection);

        return result;
    }

    private static async Task<List<EmployeeDto>> GetEmployees(GetEmployeesQuery request, IDbConnection connection)
    {
        string supervisorsCsv = await GetSupervisorsCsv(request, connection);
        string sql = BuildFilterQuery(request);
        var positions = MapStingCollectionToPositionCollection(request);

        var lookup = new Dictionary<Guid, EmployeeDto>();

        var result = await connection.QueryAsync<IntermediateEmployeeDto, Positions, EmployeeDto>(
            sql, (employee, position) =>
            {
                if (!lookup.TryGetValue(employee.Id, out var employeeDto))
                {
                    employeeDto = new EmployeeDto(employee.Id, employee.FirstName,
                        employee.LastName, employee.Email, employee.City, supervisorsCsv, []);
                    lookup.Add(employee.Id, employeeDto);
                }

                employeeDto.Positions.Add(Enum.GetName(position)!);

                return employeeDto;
            }, new
            {
                request.RestaurantId,
                FirstNameFilter = FormatFilter(request.FirstNameFilter),
                LastNameFilter = FormatFilter(request.LastNameFilter),
                EmailFilter = FormatFilter(request.EmailFilter),
                CityFilter = FormatFilter(request.CityFilter),
                PositionFilter = positions
            },
            splitOn: "Positions");

        return result.Distinct().ToList();
    }

    private static List<Positions>? MapStingCollectionToPositionCollection(GetEmployeesQuery request)
    {
        return request.PositionFilter?.Where(position => Enum.TryParse<Positions>(position, ignoreCase: true, out _))
            .Select(position => Enum.Parse<Positions>(position, ignoreCase: true)).ToList();
    }

    private static string FormatFilter(string? filter)
        => $"%{filter}%";

    private static string BuildFilterQuery(GetEmployeesQuery request)
    {
        var sql = $"""
            SELECT e.EmployeeId AS {nameof(EmployeeDto.Id)},
                e.FirstName AS {nameof(EmployeeDto.FirstName)},
                e.LastName AS {nameof(EmployeeDto.LastName)},
                e.Email AS {nameof(EmployeeDto.Email)},
                e.City AS {nameof(EmployeeDto.City)},
                ep.Position AS {nameof(EmployeeDto.Positions)}
            FROM dbo.Employees e
            LEFT JOIN dbo.EmployeePositions ep on e.EmployeeId = ep.EmployeeId
            WHERE e.RestaurantId = @RestaurantId
                AND (e.FirstName LIKE @firstNameFilter)
                AND (e.LastName LIKE @lastNameFilter)
                AND (e.Email LIKE @emailFilter)
                AND (e.City LIKE @cityFilter)
            """;

        var filterPositionsSql = """
            AND EXISTS (
                SELECT 1
                FROM dbo.EmployeePositions
                WHERE EmployeeId = e.EmployeeId AND Position IN @positionFilter
            )
            """;

        if (request.PositionFilter?.Any() == true)
        {
            sql += filterPositionsSql;
        }

        return sql;
    }

    private static async Task<string> GetSupervisorsCsv(GetEmployeesQuery request, IDbConnection connection)
    {
        var supervisors = await connection.QueryAsync(
                    @"SELECT FirstName, LastName 
                    FROM dbo.Managers m
                    JOIN dbo.Users u on m.UserId = u.Id
                    WHERE RestaurantId = @RestaurantId", new { request.RestaurantId });
        var supervisorsCsv = string.Join(", ", supervisors.Select(supervisor => $"{supervisor.FirstName} {supervisor.LastName}"));
        return supervisorsCsv;
    }

    private class IntermediateEmployeeDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string City { get; set; } = null!;
        public List<Positions> Positions { get; set; } = [];
    }
}